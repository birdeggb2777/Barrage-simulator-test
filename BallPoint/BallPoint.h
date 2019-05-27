// BallPoint.h

#pragma once

using namespace System;

namespace BallPoint {

	public ref class BallDraw
	{
	public:
		unsigned char* BallPF;
		unsigned char* checkstart;
		unsigned char* checkendd;
		unsigned char* BallByte;
		int BallH, BallW, width;
		BallDraw( unsigned char*st, unsigned char*end, int  W, int H, int Wid)
		{
			checkstart = st;
			checkendd = end;
			BallW = W;
			BallH = H;
			width = Wid;
			BallByte = new unsigned char[10000];
		}
		void BallCount(unsigned char* OriginBallByte,int MyByteCount)
		{
			int i = 0;
			BallByte = new unsigned char[MyByteCount];
			for (i = 0; i < MyByteCount; i += 4)
			{
				BallByte[i] = *(OriginBallByte+i);
				BallByte[i+1] = *(OriginBallByte + i + 1);
				BallByte[i + 2] = *(OriginBallByte + i + 2);
				BallByte[i + 3] = *(OriginBallByte + i + 3);
			}
		}
		void Check(unsigned char* st, unsigned char* end)
		{
			checkstart = st;
			checkendd = end;
		}
		void Check(int w,int h)
		{
			BallW = w;
			BallH = h;
		}
		inline void draw(unsigned char* point)
		{		
			unsigned char* point2 = point;
			unsigned char* BallPF2 = BallByte;
			int iW, iH;
			unsigned char color3;
			for (iH = 0; iH < BallH; iH++)
			{
				for (iW = 0; iW < BallW; iW++)
				{
					point2 = (point + ((iH >>1) * width + (iW>>1)) * 3);
					BallPF2 = (BallByte + (iH * BallW + iW) * 4);
					color3 = *(BallPF2 + 3);
					if (!(point2 <= checkstart || point2 >= checkendd))
					{
						*(point2) = ((*(point2 + 0) * (255 - color3) / 255 + *(BallPF2 + 0) * color3 / 255));
						*(point2 + 1) = ((*(point2 + 1) * (255 - color3) / 255 + *(BallPF2 + 1) * color3 / 255));
						*(point2 + 2) = ((*(point2 + 2) * (255 - color3) / 255 + *(BallPF2 + 2) * color3 / 255));
					}

				}
			}
		}


		// TODO:  在此加入這個類別的方法。
	};
}
