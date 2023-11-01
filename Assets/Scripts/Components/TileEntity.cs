using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileEntity : MonoBehaviour {
  private const float SPARK_ROTATE_SPEED = 60.0f;

  [SerializeField] private GameObject spark;

  private Tile owner;
  private bool showSpark;
  private readonly List<Point> targetedTiles = new List<Point>();

  void OnEnable() {
    this.AddObserver(TargetTiles, Notifications.TILE_WALL_CHANGED);
    this.AddObserver(TargetTiles, Notifications.BOMB_PRIMED, owner);
    this.AddObserver(ClearTargets, Notifications.BOMB_REMOVED, owner);
    this.AddObserver(DamageTargets, Notifications.BOMB_EXPLODED, owner);
  }
  void OnDisable() {
    this.RemoveObserver(TargetTiles, Notifications.TILE_WALL_CHANGED);
    this.RemoveObserver(TargetTiles, Notifications.BOMB_PRIMED, owner);
    this.RemoveObserver(ClearTargets, Notifications.BOMB_REMOVED, owner);
    this.RemoveObserver(DamageTargets, Notifications.BOMB_EXPLODED, owner);
  }

  protected void Awake() {
    owner = GetComponent<Tile>();
    if (!owner) owner = GetComponentInParent<Tile>();
  }
  protected void Start() {
    Render();
  }
  protected void Update() {
    if (showSpark) {
      spark.transform.localEulerAngles = Vector3.forward * Time.time * SPARK_ROTATE_SPEED;
      transform.localScale = Vector3.one * (1.0f + 0.035f * Mathf.Sin(Time.time * 8.0f));
    }
  }
  public void Render() {
    var coordinates = new Vector2(3, 3);

    switch (owner.status) {
      case TileStatus.BoxBomb:
        coordinates = new Vector2(0, owner.countdown == 2 ? 1 : 2);
        break;
      case TileStatus.StarBomb:
        coordinates = new Vector2(1, owner.countdown == 2 ? 1 : 2);
        break;
    }

    MeshFilter mFilter = GetComponent<MeshFilter>();
    List<Vector2> uvs = new List<Vector2>(mFilter.mesh.uv);
    float t = 0.25f;
    uvs[0] = new Vector2(coordinates.x * t, coordinates.y * t);
    uvs[1] = new Vector2(coordinates.x * t + t, coordinates.y * t);
    uvs[2] = new Vector2(coordinates.x * t, coordinates.y * t + t);
    uvs[3] = new Vector2(coordinates.x * t + t, coordinates.y * t + t);

    Mesh mesh = mFilter.mesh;
    mesh.uv = uvs.ToArray();

    showSpark = owner.countdown == 1;
    spark.gameObject.SetActive(showSpark);
    transform.localScale = Vector3.one;
  }
  public void DamageTargets(object sender = null, object data = null) {
    if (data is int val) {
      Debug.Log("Damage for " + val);
    }

    ClearTargets();
  }
  public void TargetTiles(object sender = null, object data = null) {
    ClearTargets();

    if (owner.IsBomb()) targetedTiles.Add(owner.pos);

    if (targetedTiles.Count > 0) {
      gameObject.PostNotification(Notifications.BOMB_ADD_TARGET, new TelegraphInfo(gameObject, targetedTiles, true));
    }
  }
  public void ClearTargets(object sender = null, object data = null) {
    if (targetedTiles.Count > 0) {
      gameObject.PostNotification(Notifications.BOMB_REMOVE_TARGET, new TelegraphInfo(gameObject, targetedTiles, true));
      targetedTiles.Clear();
    }
  }
}
