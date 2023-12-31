using System.Collections;
using System.Collections.Generic;
using MEC;
using UnityEngine;

public class GameStateBoardInit : GameState {
  public override void Enter() {
    base.Enter();
    mainRoutine = Timing.RunCoroutine(_Start().CancelWith(gameObject));
  }

  IEnumerator<float> _Start() {
    GameObject prefab = owner.boardMode == GameController.BoardMode.Stage ?
     owner.stageControllerPrefab :
     owner.editControllerPrefab;

    GameObject boardController = Instantiate(prefab);
    boardController.transform.SetParent(owner.transform);
    boardController.transform.localPosition = Vector3.zero;
    boardController.transform.localEulerAngles = Vector3.zero;
    boardController.transform.localScale = Vector3.one;

    owner.currentBoard = boardController.GetComponent<StateMachineBoard>();
    owner.currentBoard.mapToLoad = owner.campaignMaps[owner.currentMap];
    owner.currentBoard.subdirectory = "/Campaign";

    List<HelpInfo> helps = new List<HelpInfo>(boardController.GetComponentsInChildren<HelpInfo>());
    foreach (HelpInfo help in helps) {
      if (help.mapsToInvertOn.Contains(owner.currentMap)) {
        help.gameObject.SetActive(!help.activeByDefault);
      } else {
        help.gameObject.SetActive(help.activeByDefault);
      }
    }

    yield return 0;
    owner.ChangeState<GameStateBoardRunning>();
  }
}
