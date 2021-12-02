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

void Face_float (float2 UV, float2 offsetUV, float4 outlineColor, float4 irisColor, float4 eyesColor, 
	float eyesDistance, float eyesSize, float eyesShape, float irisPositionX, float irisPositionY, float irisSize, float irisShape, float bossEyed, 
	float outlineSize, bool mouthOpen, float mouthSize, float mouthWidth, float mouthHeight, float mouthThickness, float smile, 
	float topEyelid, float bottomEyelid, float rings, float straightEyelid,
	out float alpha, out float4 Out)
{
	UV += offsetUV;
	float2 eyeUV = float2(abs(UV.x*2.0 - 1.0), UV.y);
	float2 irisUV = float2(lerp(frac(UV.x * 2.0), abs(UV.x * 2.0 - 1.0), bossEyed), UV.y);
	float2 mouthUV = eyeUV;

	eyeUV.y -= 0.5;
	eyeUV.y *= eyesShape;
	eyeUV.y += 0.5;

	irisUV.y -= 0.5;
	irisUV.y *= irisShape;
	irisUV.y += 0.5;
	float4 col = 0.0;

	float eyes = step(distance(float2(eyesDistance, 0.5), eyeUV), eyesSize);

	eyes = straightEyelid ? eyes * step(eyeUV.y, 1.0- topEyelid) * step(bottomEyelid, eyeUV.y) 
		: eyes*step(eyesSize*2.0, distance(float2(eyesDistance, 0.5 + topEyelid),eyeUV))*step(eyesSize * 2.0, distance(float2(eyesDistance, 0.5 - bottomEyelid), eyeUV));

	float iris = step(distance(float2(bossEyed > 0 ? eyesDistance 
		: (UV.x > 0.5 ? eyesDistance : 1.0 - eyesDistance), 0.5) + float2(irisPositionX, irisPositionY) * eyesSize, irisUV), eyesSize * irisSize);

	float outlineEyes = rings ? step(distance(float2(eyesDistance, 0.5), eyeUV), eyesSize + outlineSize)
		: straightEyelid ? step(distance(float2(eyesDistance, 0.5), eyeUV), eyesSize + outlineSize) * step(eyeUV.y, 1.0 - topEyelid + outlineSize) * step(bottomEyelid - outlineSize, eyeUV.y)
			: step(distance(float2(eyesDistance, 0.5), eyeUV), eyesSize + outlineSize) * step(eyesSize*2.0-outlineSize, distance(float2(eyesDistance, 0.5 + topEyelid), eyeUV)) * step(eyesSize * 2.0-outlineSize, distance(float2(eyesDistance, 0.5 - bottomEyelid), eyeUV));

	float mouth = mouthOpen ? step(distance(float2(mouthWidth, mouthHeight), mouthUV), mouthSize)
		: smile ? step(Bezier(UV, float2(0.5 - mouthWidth, mouthHeight), float2(0.5, mouthHeight + smile), float2(0.5 + mouthWidth, mouthHeight)), mouthThickness)
			: step(Segment(UV, float2(0.5 - mouthWidth, mouthHeight), float2(0.5 + mouthWidth, mouthHeight)), mouthThickness);

	alpha = outlineEyes + mouth;

	if (eyes > 0) {
		if (iris > 0) {
			col.rgb = irisColor;
		}
		else {
			col.rgb = eyesColor;
		}
	}
	else {
		col.rgb = outlineColor;
	}

	//col.a = alpha;
	Out = col;
}
