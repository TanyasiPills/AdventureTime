import { DiscordSDK } from "@discord/embedded-app-sdk";
import './style.css'

function Log(...message) {
  console.log("<<client>> ", ...message);
}

let auth;
let tokenIn;

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
  }).then(async unityInstance => {
    Log(auth)
    if (unityInstance) {
      Log("bob sending message");
      unityInstance.SendMessage("Bridge", "SetUserData", JSON.stringify({
        "username": auth.user.global_name,
        "iconUrl": `https://cdn.discordapp.com/avatars/${auth.user.id}/${auth.user.avatar}.png?size=256`,
        "access_token": auth.access_token,
        "session_token": tokenIn
      }));
    }
  });
});

async function setupDiscordSdk() {

Log("Setting up Discord SDK");
await discordSdk.ready();

tokenIn = localStorage.getItem("sessionToken");
const needTokenAuth = true;


if (tokenIn) {
    console.log("Found token:", tokenIn);
    const response = await fetch("/api/token/validate", {
        methos: "POST",
        headers: {
            "Content-Type": "application/json",
        },
        body: JSON.stringify({
            token : tokenIn
        }),
    });

    const result = await resonse.json();
    needTokenAuth = result.need;
    let authToken = result.auth;
    auth = await discordSdk.commands.authenticate({
      authToken,
    });
}

if(needTokenAuth){
    console.log("Token not found or invalid, asking for auth");
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
}
    Log("Discord SDK is ready");
}

