using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotherController : MonoBehaviour
{
	Camera mainCamera;
	RaycastHit hit = new RaycastHit();

	const float k_Spring = 5000.0f;
	const float k_Damper = 5.0f;
	const float k_Drag = 10.0f;
	const float k_AngularDrag = 5.0f;
	const float k_Distance = 0.2f;
	const bool k_AttachToCenterOfMass = false;
	const float k_PunchForce = 50f;

	private SpringJoint m_SpringJoint;

	Coroutine DragCoroutine;

	[SerializeField] private LayerMask dragLayerMask;
	[SerializeField] private LayerMask punchLayerMask;

	[Header("Effects")]
	[SerializeField] AudioSource audioSource;
	[SerializeField] private AudioClip punchAudio, grabAudio;
	[SerializeField] private GameObject punchEffectPrefab;


	private void Awake()
	{
		mainCamera = FindCamera();
	}

	private void Update()
	{
		if (!Input.GetMouseButtonDown(0))
		{
			return;
		}

		if(!Punch())
			Drag();
	}


	private bool Drag()
	{
		if (
			!Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition).origin,
			mainCamera.ScreenPointToRay(Input.mousePosition).direction, out hit, 100,
			dragLayerMask))
		{
			return false;
		}

		if (!hit.rigidbody || hit.rigidbody.isKinematic)
		{
			return false;
		}

        if (Input.GetMouseButton(0))
        {
			audioSource.PlayOneShot(grabAudio);

			if (hit.rigidbody.TryGetComponent(out BotherAnimation botherAnimation))
			{
				botherAnimation.Trigger();
			}
		}

		if (!m_SpringJoint)
		{
			var go = new GameObject("Rigidbody dragger");
			Rigidbody body = go.AddComponent<Rigidbody>();
			m_SpringJoint = go.AddComponent<SpringJoint>();
			body.isKinematic = true;
		}

		m_SpringJoint.transform.position = hit.point;
		m_SpringJoint.anchor = Vector3.zero;

		m_SpringJoint.spring = k_Spring;
		m_SpringJoint.damper = k_Damper;
		m_SpringJoint.maxDistance = k_Distance;
		m_SpringJoint.connectedBody = hit.rigidbody;

		DragCoroutine = StartCoroutine("DragObject", hit.distance);
		return true;
	}


	private IEnumerator DragObject(float distance)
	{
		var oldDrag = m_SpringJoint.connectedBody.drag;
		var oldAngularDrag = m_SpringJoint.connectedBody.angularDrag;
		m_SpringJoint.connectedBody.drag = k_Drag;
		m_SpringJoint.connectedBody.angularDrag = k_AngularDrag;
		while (Input.GetMouseButton(0))
		{
			var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
			m_SpringJoint.transform.position = ray.GetPoint(distance);
			yield return null;
		}
		if (m_SpringJoint.connectedBody)
		{
			m_SpringJoint.connectedBody.drag = oldDrag;
			m_SpringJoint.connectedBody.angularDrag = oldAngularDrag;
			m_SpringJoint.connectedBody = null;
		}
	}

	private bool Punch()
    {
		//Se que es horrible tirar dos raycast y repetir parte del codigo pero bueno.
		if (Input.GetMouseButtonDown(0))
        {
			if (
				!Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition).origin,
				mainCamera.ScreenPointToRay(Input.mousePosition).direction, out hit, 100,
				punchLayerMask))
			{
				return false;
			}

			if (!hit.rigidbody)
			{
				return false;
			}

			if(hit.rigidbody.TryGetComponent(out BotherAnimation botherAnimation))
            {
				botherAnimation.Trigger();
            }

			Instantiate(punchEffectPrefab, hit.point, Quaternion.identity);
			audioSource.PlayOneShot(punchAudio);
			hit.rigidbody.AddForce(mainCamera.transform.forward * k_PunchForce, ForceMode.Impulse);
			return true;
		}
		return false;
	}

	private Camera FindCamera()
	{
		if (GetComponent<Camera>())
		{
			return GetComponent<Camera>();
		}

		return Camera.main;
	}
}

