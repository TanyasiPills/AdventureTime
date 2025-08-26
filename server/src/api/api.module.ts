import { Module } from '@nestjs/common';
import { ApiService } from './api.service';
import { ConfigModule } from '@nestjs/config';
import { ApiController } from './api.controller';
import { PrismaService } from 'src/prisma.service';

@Module({
  controllers: [ApiController],
    imports: [
    ConfigModule.forRoot({
      isGlobal: true
    })
  ],
  providers: [ApiService, PrismaService],
  exports: [ApiService]
})
export class ApiModule {}
