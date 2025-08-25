import { DiscordSDK } from "@discord/embedded-app-sdk";
import './style.css'

function Log(...message) {
  console.log("<<client>> ", ...message);
}

let auth;

const discordSdk = new DiscordSDK(import.meta.env.VITE_DISCORD_CLIENT_ID);

var canvas = document.querySelector("#unity-canvas");
canvas.style.width = "100%";
canvas.style.height = "100%";
canvas.style.position = "fixed";

var meta = document.createElement('meta');
meta.name = 'viewport';
meta.content = 'width=device-width, height=device-height, initial-scale=1.0, user-scalable=no, shrink-to-fit=yes';
document.getElementsByTagName('head')[0].appendChild(meta);

document.body.style.textAlign = "left";

setupDiscordSdk().then(() => {
  createUnityInstance(document.querySelector("#unity-canvas"), {
    arguments: [],
    dataUrl: "Build/Build/Build.data.gz",
    frameworkUrl: "Build/Build/Build.framework.js.gz",
    codeUrl: "Build/Build/Build.wasm.gz",
    streamingAssetsUrl: "StreamingAssets",
    companyName: "GézaVenturesStudio",
    productName: "Adventure",
    productVersion: "0.1.0",
    // matchWebGLToCanvasSize: false, // Uncomment this to separately control WebGL canvas render size and DOM element size.
    // devicePixelRatio: 1, // Uncomment this to override low DPI rendering on high DPI displays.
  }).then(async unityInstance => {
    Log(auth)

    const member = await fetch(`https://discord.com/api/v10/users/@me/guilds/${discordSdk.guildId}/member`, {
      headers: {
        Authorization: `Bearer ${auth.access_token}`,
      },
    }).then((response) => response.json());

    let username = member?.nick ?? auth.user.global_name;
    let iconUrl = "";

    if (member?.avatar != null) {
      iconUrl = `https://cdn.discordapp.com/guilds/${discordSdk.guildId}/members/${auth.user.id}/avatars/${member.avatar}.webp?size=256`;
    } else {
      iconUrl = `https://cdn.discordapp.com/avatars/${auth.user.id}/${auth.user.avatar}.png?size=256`;
    }

    if (unityInstance) {
      Log("bob sending message");
      unityInstance.SendMessage("Bridge", "SetUserData", JSON.stringify({
        "username": username,
        "iconUrl": iconUrl,
        "access_token": auth.access_token
      }));
    }
  });
});



async function appendVoiceChannelName() {
  const app = document.querySelector('#app');

  let activityChannelName = 'Unknown';

  // Requesting the channel in GDMs (when the guild ID is null) requires
  // the dm_channels.read scope which requires Discord approval.
  if (discordSdk.channelId != null && discordSdk.guildId != null) {
    // Over RPC collect info about the channel
    const channel = await discordSdk.commands.getChannel({ channel_id: discordSdk.channelId });
    if (channel.name != null) {
      activityChannelName = channel.name;
    }
  }

  // Update the UI with the name of the current voice channel
  const textTagString = `Activity Channel: "${activityChannelName}"`;
  const textTag = document.createElement('p');
  textTag.innerHTML = textTagString;
  app.appendChild(textTag);
}

async function appendGuildAvatar() {
  const app = document.querySelector('#app');

  // 1. From the HTTP API fetch a list of all of the user's guilds
  const guilds = await fetch(`https://discord.com/api/v10/users/@me/guilds`, {
    headers: {
      // NOTE: we're using the access_token provided by the "authenticate" command
      Authorization: `Bearer ${auth.access_token}`,
      'Content-Type': 'application/json',
    },
  }).then((response) => response.json());

  // 2. Find the current guild's info, including it's "icon"
  const currentGuild = guilds.find((g) => g.id === discordSdk.guildId);

  // 3. Append to the UI an img tag with the related information
  if (currentGuild != null) {
    const guildImg = document.createElement('img');
    guildImg.setAttribute(
      'src',
      // More info on image formatting here: https://discord.com/developers/docs/reference#image-formatting
      `https://cdn.discordapp.com/icons/${currentGuild.id}/${currentGuild.icon}.webp?size=128`
    );
    guildImg.setAttribute('width', '128px');
    guildImg.setAttribute('height', '128px');
    guildImg.setAttribute('style', 'border-radius: 50%;');
    app.appendChild(guildImg);
  }
}

async function setupDiscordSdk() {
  Log("Setting up Discord SDK");
  await discordSdk.ready();

  // Authorize with Discord Client
  const { code } = await discordSdk.commands.authorize({
    client_id: import.meta.env.VITE_DISCORD_CLIENT_ID,
    response_type: "code",
    state: "",
    prompt: "none",
    scope: [
      "identify",
      "guilds",
      "guilds.members.read"
    ],
  });

  const response = await fetch("/api/token", {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify({
      code,
    }),
  });
  const { access_token } = await response.json();

  // Authenticate with Discord client (using the access_token)
  auth = await discordSdk.commands.authenticate({
    access_token,
  });

  if (auth == null) {
    Log("Authenticate command failed");
    throw new Error("Authenticate command failed");
  }

  Log("Discord SDK is ready");
}

//<img src="${rocketLogo}" class="logo" alt="Discord" />