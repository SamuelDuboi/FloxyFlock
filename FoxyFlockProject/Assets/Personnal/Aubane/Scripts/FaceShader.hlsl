void Face_float (float2 UV, float2 offsetUV, float4 irisColor, float4 eyesColor, float eyesDistance, float eyesSize, float eyesShape, float bossEyed, float2 irisPosition, float irisSize, float irisShape, out float alpha, out float4 Out)
{
	UV += offsetUV;
	float2 eyeUV = float2(abs(UV.x*2.0 - 1.0), UV.y);
	float2 irisUV = eyeUV; //float2(lerp(frac(UV.x * 2.0), abs(UV.x * 2.0 - 1.0), bossEyed, UV.y));

	eyeUV.y -= 0.5;
	eyeUV.y *= eyesShape;
	eyeUV.y += 0.5;

	irisUV.y -= 0.5;
	irisUV.y *= irisShape;
	irisUV.y += 0.5;
	float4 col = 0.0;

	float eyes = step(distance(float2(eyesDistance, 0.5), eyeUV), eyesSize);
	float iris = step(distance(float2(eyesDistance, 0.5) + irisPosition * eyesSize, irisUV), eyesSize * irisSize);

	alpha = eyes;
	col.rgb = iris > 0 ? irisColor : eyes * eyesColor;
	col.a = alpha;
	Out = col;
}
