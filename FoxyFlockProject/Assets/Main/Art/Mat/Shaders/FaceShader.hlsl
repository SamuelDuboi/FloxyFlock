float Segment(float2 p, float2 a, float2 b) {
	float2 pa = p - a, ba = b - a;
	float h = clamp(dot(pa, ba) / dot(ba, ba), 0.0, 1.0);
	return length(pa - ba * h);
} 

float Bezier(in float2 pos, in float2 A, in float2 B, in float2 C)
{
	float2 a = B - A;
	float2 b = A - 2.0 * B + C;
	float2 c = a * 2.0;
	float2 d = A - pos;
	float kk = 1.0 / dot(b, b);
	float kx = kk * dot(a, b);
	float ky = kk * (2.0 * dot(a, a) + dot(d, b)) / 3.0;
	float kz = kk * dot(d, a);
	float res = 0.0;
	float p = ky - kx * kx;
	float p3 = p * p * p;
	float q = kx * (2.0 * kx * kx - 3.0 * ky) + kz;
	float h = q * q + 4.0 * p3;

	if (h >= 0.0)
	{
		h = sqrt(h);
		float2 x = (float2(h, -h) - q) / 2.0;
		float2 uv = sign(x) * pow(abs(x), float2(1.0 / 3.0, 1.0/3.0));
		float t = clamp(uv.x + uv.y - kx, 0.0, 1.0);
		res = dot(d + (c + b * t) * t, d + (c + b * t) * t);
	}
	else 
	{
		float z = sqrt(-p);
		float v = acos(q / (p * z * 2.0)) / 3.0;
		float m = cos(v);
		float n = sin(v) * 1.732050808;
		float3 t = clamp(float3(m + m, -n - m, n - m) * z - kx, 0.0, 1.0);
		res = min(dot(d + (c + b * t.x) * t.x, d + (c + b * t.x) * t.x), 
			dot(d + (c + b * t.y) * t.y, d + (c + b * t.y) * t.y));
		//the third root cannot be the closest
		//res = min(res,dot2(d+(c+b*t.z)*t.z));
	}
	return sqrt(res);
}

float RoundedRectangle(float2 UV, float Width, float Height, float Radius, float2 offset)
{
	UV = UV + offset;
	Radius = max(min(min(abs(Radius * 2), abs(Width)), abs(Height)), 1e-5);
	float2 uv = abs(UV * 2 - 1) -float2(Width, Height) + Radius;
	float d = length(max(0, uv)) / Radius;
	float fwd = max(fwidth(d), 1e-5);
	return saturate((1 - d) / fwd);
}

void Face_float(float2 UV, float2 offsetUV, float outlineSize, float4 outlineColor, float4 irisColor, float4 eyesColor, float4 mouthColor, float4 tongueColor, float4 teethColor,
	float eyesDistance, float eyesSize, float eyesShape, float irisPositionX, float irisPositionY, float irisSize, float irisShape, bool bossEyed, bool invertIris, bool irisLight,
	float topEyelid, float bottomEyelid, bool rings, bool straightEyelid,
	bool mouthOpen, float mouthWidth, float mouthHeight, float mouthThickness, float smile,
	float openMouthSize, float openMouthShapeX, float openMouthShapeY, float tongueSize, float tonguePositionY, float straightMouth, float topLips, float bottomLips, float lipsShapeX, 
	bool withTeeth, float teethWidth, float teethHeight, float teethCorners,
	out float alpha, out float4 Out)
{
	UV += offsetUV;
	float2 eyeUV = float2(abs(UV.x*2.0 - 1.0), UV.y);
	float2 irisUV = float2(lerp(frac(UV.x * 2.0), abs(UV.x * 2.0 - 1.0), bossEyed), UV.y);
	float2 mouthUV = eyeUV;
	float2 openMouthUV = UV;
	float2 lipsUV = UV;

	eyeUV.y -= 0.5;
	eyeUV.y *= eyesShape;
	eyeUV.y += 0.5;

	irisUV.y -= 0.5;
	irisUV.y *= irisShape;
	irisUV.y += 0.5;

	openMouthUV.x -= 0.5;
	openMouthUV.x *= openMouthShapeX;
	openMouthUV.x += 0.5;

	lipsUV.y = openMouthUV.y;
	lipsUV.x -= 0.5;
	lipsUV.x *= (openMouthShapeX - lipsShapeX);
	lipsUV.x += 0.5;

	float4 col = 0.0;
	float3 colEyes = 0.0;
	float3 colMouth = 0.0;

	float lightOffsetX = 0.0035;
	float lightOffsetY = 0.35;

	float eyes = step(distance(float2(eyesDistance, 0.5), eyeUV), eyesSize);

	eyes = straightEyelid ? eyes * step(eyeUV.y, 1.0- topEyelid) * step(bottomEyelid, eyeUV.y) 
		: eyes*step(eyesSize*2.0, distance(float2(eyesDistance, 0.5 + topEyelid),eyeUV))*step(eyesSize * 2.0, distance(float2(eyesDistance, 0.5 - bottomEyelid), eyeUV));

	float iris = step(distance(float2(bossEyed ? eyesDistance
		: (UV.x > 0.5 ? eyesDistance : 1.0 - eyesDistance), 0.5) + float2(irisPositionX, (bossEyed ? (invertIris ? (UV.x > 0.5 ? irisPositionY : -irisPositionY) : irisPositionY) : irisPositionY)) * eyesSize, irisUV), eyesSize* irisSize);

	float lightInIris = irisLight ? step(distance(float2(bossEyed ? eyesDistance + lightOffsetX
		: (UV.x > 0.5 ? eyesDistance + lightOffsetX : 1.0 - eyesDistance + lightOffsetX), 0.5) 
		+ float2(irisPositionX + lightOffsetX, (bossEyed ? (invertIris ? (UV.x > 0.5 ? irisPositionY + lightOffsetY : -irisPositionY + lightOffsetY) : irisPositionY + lightOffsetY) : irisPositionY + lightOffsetY)) * eyesSize, irisUV), eyesSize * irisSize * 0.3)
		: 0.0;

	float outlineEyes = rings ? step(distance(float2(eyesDistance, 0.5), eyeUV), eyesSize + outlineSize)
		: straightEyelid ? step(distance(float2(eyesDistance, 0.5), eyeUV), eyesSize + outlineSize) * step(eyeUV.y, 1.0 - topEyelid + outlineSize) * step(bottomEyelid - outlineSize, eyeUV.y)
			: step(distance(float2(eyesDistance, 0.5), eyeUV), eyesSize + outlineSize) * step(eyesSize*2.0-outlineSize, distance(float2(eyesDistance, 0.5 + topEyelid), eyeUV)) * step(eyesSize * 2.0-outlineSize, distance(float2(eyesDistance, 0.5 - bottomEyelid), eyeUV));

	float mouth = mouthOpen ? 
		straightMouth ? step(distance(float2(0.5, mouthHeight), openMouthUV), mouthWidth) * step(openMouthUV.y, mouthHeight)
			: step(distance(float2(0.5, mouthHeight), openMouthUV), mouthWidth) * step(mouthWidth * 2.0, distance(float2(0.5, mouthHeight + topLips), lipsUV)) * step(mouthWidth * 2.0, distance(float2(0.5, mouthHeight - bottomLips), lipsUV))
		: smile ? step(Bezier(UV, float2(0.5 - mouthWidth, mouthHeight), float2(0.5, mouthHeight + smile), float2(0.5 + mouthWidth, mouthHeight)), mouthThickness)
			: step(Segment(UV, float2(0.5 - mouthWidth, mouthHeight), float2(0.5 + mouthWidth, mouthHeight)), mouthThickness);

	float tongue = step(distance(float2(0.5, mouthHeight - openMouthShapeY * mouthWidth) + float2(0.0, tonguePositionY) * tongueSize, openMouthUV), tongueSize);

	float outlineMouth = mouthOpen ? 
		straightMouth ? step(distance(float2(0.5, mouthHeight), openMouthUV), mouthWidth + outlineSize) * step(openMouthUV.y, mouthHeight + outlineSize) 
			: step(distance(float2(0.5, mouthHeight), openMouthUV), mouthWidth + outlineSize) * step(mouthWidth * 2.0 - outlineSize, distance(float2(0.5, mouthHeight + topLips), lipsUV)) * step(mouthWidth * 2.0 - outlineSize, distance(float2(0.5, mouthHeight - bottomLips), lipsUV))
		: mouth;

	float teeth = withTeeth ? RoundedRectangle(UV, teethWidth, teethHeight, teethCorners, float2(0.0,0.5-mouthHeight)) : 0.0;

	alpha = outlineEyes + outlineMouth;

	if (eyes > 0) {
		if (iris > 0) {
			colEyes = irisColor;
			if (lightInIris > 0) {
				colEyes = 1.0;
			}
		}
		else {
			colEyes = eyesColor;
		}
	}

	if (mouthOpen == true) {
		if (mouth > 0) {
			colMouth = mouthColor;
			if (tongue > 0) {
				colMouth = tongueColor;
			}
			if (teeth > 0) {
				colMouth = teethColor;
			}
		}
	}
	else { colMouth = outlineColor; }


	col.rgb = lerp(outlineColor, lerp(colEyes, colMouth, mouth), eyes + mouth);
	
	
	Out = col;
}
