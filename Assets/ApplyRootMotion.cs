using UnityEngine;
using System.Collections;

public class ApplyRootMotion : MonoBehaviour 
{
	public Transform m_flagObject;	// 用来测试位置的一个对象

	//-- Root Motion 控制变量
	Transform m_rootBone;
    Vector3 m_lastRootPos;
    Vector3 m_rootMotion;
    int m_lastAnimTime;

	void Start () 
	{
		//-- 从SkinnedMeshRenderer中读取Root Bone
		SkinnedMeshRenderer skinMesh = this.gameObject.GetComponentInChildren<SkinnedMeshRenderer>();
        m_rootBone = skinMesh.rootBone;

        //-- 变量初始化
        m_rootMotion = Vector3.zero;
        m_lastRootPos = m_rootBone.localPosition;
        m_lastAnimTime = 0;
	}
	
	void Update () 
	{
		//-- Apply Root Motion
		Vector3 nextPos = this.transform.position + m_rootMotion;
        this.transform.position = nextPos;

        //-- 测试代码：更新测试物体的位置
        Vector3 flagPos = m_flagObject.position;
        flagPos.x = nextPos.x;
        flagPos.z = nextPos.z;
        m_flagObject.position = flagPos;

        //-- 测试代码：更新摄像机
        Camera.main.transform.LookAt(this.transform);
	}

	void LateUpdate()
    {
		AnimationState animState = this.animation["walking"];

		if ((int)animState.normalizedTime > m_lastAnimTime)
		{
        	//-- 动画循环处理
        	m_lastRootPos = m_rootBone.localPosition;
        	m_rootMotion = Vector3.zero;
        }
        else
        {
        	//-- 计算当前帧的Root Motion
        	Vector3 rootPos = m_rootBone.localPosition;
        	m_rootMotion = rootPos - m_lastRootPos;
        	m_lastRootPos = rootPos;
        	rootPos.x = 0;
        	rootPos.z = 0;
        	m_rootMotion.y = 0;
        	m_rootBone.localPosition = rootPos;
        }
        m_lastAnimTime = (int)animState.normalizedTime;
    }
}
