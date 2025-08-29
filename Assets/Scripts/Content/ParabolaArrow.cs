using System.Collections;
using UnityEngine;
using static Define;

public class ParabolaArrow : MonoBehaviour
{
	[SerializeField] bool rotateToPath = true;
	[SerializeField] int samples = 32;            // 16~64 권장
	[SerializeField] bool useUnscaledTime = false;

	Vector2 S, M, E, C;                           // 시작, 중간(통과점), 끝, 제어점
	float speed;                                   // 초당 이동 거리(등속)
	float totalLen, traveled;
	float[] cum;                                   // 누적 길이 테이블

	float DT => useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;

	public void Launch(Vector2 start, Vector2 mid, Vector2 end, float speedPerSec)
	{
		S = start; M = mid; E = end;
		speed = Mathf.Max(0.01f, speedPerSec);

		// t=0.5에서 M을 지나가도록 제어점 계산
		C = 2f * M - 0.5f * (S + E);

		BuildLUT();
		traveled = 0f;
		transform.position = S;
		enabled = totalLen > 0f;
	}

	void Update()
	{
		if (totalLen <= 0f) return;

		traveled += speed * DT;
		if (traveled >= totalLen)
		{
			transform.position = E;
			enabled = false;
			return;
		}

		float t = TFromArcLen(traveled);
		Vector2 p = Bezier(t);
		transform.position = p;

		if (rotateToPath)
		{
			float t2 = Mathf.Min(1f, t + 1f / (samples * 1.5f));
			Vector2 p2 = Bezier(t2);
			Vector2 dir = p2 - p;
			if (dir.sqrMagnitude > 1e-6f) transform.right = dir.normalized; // 스프라이트 +X가 전방
		}
	}

	Vector2 Bezier(float t)
	{
		t = Mathf.Clamp01(t);
		float u = 1f - t;
		return u * u * S + 2f * u * t * C + t * t * E;
	}

	void BuildLUT()
	{
		samples = Mathf.Max(2, samples);
		cum = new float[samples];
		Vector2 prev = Bezier(0f);
		cum[0] = 0f;
		float acc = 0f;

		for (int i = 1; i < samples; i++)
		{
			float t = i / (float)(samples - 1);
			Vector2 p = Bezier(t);
			acc += Vector2.Distance(prev, p);
			cum[i] = acc;
			prev = p;
		}
		totalLen = acc;
	}

	float TFromArcLen(float s)
	{
		if (s <= 0f) return 0f;
		if (s >= totalLen) return 1f;

		int i = 1;
		for (; i < samples; i++) if (cum[i] >= s) break;

		float s0 = cum[i - 1], s1 = cum[i];
		float u = (s1 - s0) > 1e-6f ? (s - s0) / (s1 - s0) : 0f;

		float t0 = (i - 1) / (float)(samples - 1);
		float t1 = i / (float)(samples - 1);
		return Mathf.Lerp(t0, t1, u);
	}
}