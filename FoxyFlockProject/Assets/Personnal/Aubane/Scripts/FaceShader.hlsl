float sdSegment(float2 p, float2 a, float2 b) {
	float2 pa = p - a, ba = b - a;
	float h = clamp(dot(pa, ba) / dot(ba, ba), 0.0, 1.0);
	return length(pa - ba * h);
} 

void Face_float (float2 UV, float2 offsetUV, float4 outlineColor, float4 irisColor, float4 eyesColor, float eyesDistance, float eyesSize, float eyesShape, float2 irisPosition, float irisSize, float irisShape, float bossEyed, float outlineSize, float mouthWidth, float mouthHeight, float mouthThickness, out float alpha, out float4 Out)
{
	UV += offsetUV;
	float2 eyeUV = float2(abs(UV.x*2.0 - 1.0), UV.y);
	float2 irisUV = eyeUV; // float2(lerp(frac(UV.x * 2.0), abs(UV.x * 2.0 - 1.0), bossEyed, UV.y));
	float2 mouthUV = eyeUV;

	eyeUV.y -= 0.5;
	eyeUV.y *= eyesShape;
	eyeUV.y += 0.5;

	irisUV.y -= 0.5;
	irisUV.y *= irisShape;
	irisUV.y += 0.5;
	float4 col = 0.0;

	float outlineEyes = step(distance(float2(eyesDistance, 0.5), eyeUV), eyesSize + outlineSize);
	float eyes = step(distance(float2(eyesDistance, 0.5), eyeUV), eyesSize);
	float iris = step(distance(float2(eyesDistance, 0.5) + irisPosition * eyesSize, irisUV), eyesSize * irisSize);

	float mouth = step(sdSegment(UV, float2(0.5 - mouthWidth, mouthHeight), float2(0.5 + mouthWidth, mouthHeight)), mouthThickness);

	alpha = eyes + mouth;

	//col.rgb = iris > 0 ? irisColor : eyes * eyesColor;
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

	col.a = alpha;
	Out = col;
}
