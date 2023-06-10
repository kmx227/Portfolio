using UnityEngine;
using System.Linq;
using UnityEngine.EventSystems;

public class MouseClick : MonoBehaviour
{
	[SerializeField]
	private	LayerMask			layerUnit;
	[SerializeField]
	private	LayerMask			layerGround;
	[SerializeField]
	private LayerMask layerBunker;
	[SerializeField]
	private LayerMask layerUI;

	[SerializeField] GameObject UI;

	private	Camera				mainCamera;
	private	RTSUnitController	rtsUnitController;

	private void Awake()
	{
		mainCamera			= Camera.main;
		rtsUnitController	= GetComponent<RTSUnitController>();
		UI.SetActive(false);
	}

	private void Update()
	{
		// ���콺 ���� Ŭ������ ���� ���� or ����
		if ( Input.GetMouseButtonDown(0) )
		{
			Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			RaycastHit2D hit = Physics2D.Raycast(pos, transform.forward, Mathf.Infinity, layerUnit);
			RaycastHit2D hitBunker = Physics2D.Raycast(pos, transform.forward, Mathf.Infinity, layerBunker);
			RaycastHit2D hitUI = Physics2D.Raycast(pos, transform.forward, Mathf.Infinity, layerUI);
			//Ray2D ray = new Ray2D(pos, Vector2.zero);

			// ������ �ε����� ������Ʈ�� ���� �� (=������ Ŭ������ ��)
			if (hit)
			{
				if (hit.transform.GetComponent<UnitController>() == null ) return;

				if ( Input.GetKey(KeyCode.LeftShift) )
				{
					rtsUnitController.ShiftClickSelectUnit(hit.transform.GetComponent<UnitController>());
				}
				else
				{
					rtsUnitController.ClickSelectUnit(hit.transform.GetComponent<UnitController>());
				}
			}
			else if (hitBunker)
            {
				UI.SetActive(true);
            }
			else if (hitBunker)
			{
				return;
			}
			// ������ �ε����� ������Ʈ�� ���� ��
			else
			{
				if ( !Input.GetKey(KeyCode.LeftShift))
				{
					rtsUnitController.LastSelectedUnitList = rtsUnitController.SelectedUnitList.ToList();
					rtsUnitController.DeselectAll();
				}

                if (!EventSystem.current.IsPointerOverGameObject())
                {
					UIOff();
                }
			}
		}

		// ���콺 ������ Ŭ������ ���� �̵�
		if ( Input.GetMouseButtonDown(1) )
		{
			Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			RaycastHit2D hit = Physics2D.Raycast(pos, transform.forward, Mathf.Infinity, layerGround);

			if (hit)
			{
				rtsUnitController.MoveSelectedUnits(hit.point);
			}
		}
	}

	public void UIOff()
    {
        if (UI.activeSelf)
        {
			UI.SetActive(false);
        }
    }
}

