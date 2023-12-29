using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileEntityRenderer : MonoBehaviour {
  private const float SPARK_ROTATE_SPEED = 60.0f;

  [SerializeField] private GameObject spark;
  [SerializeField] private Digit digit;

  private TileEntity owner;
  private bool showSpark;
  public Point pos { get { return owner.pos; } }
  public int bombValue { get { return owner.bombValue; } }
  public int countdown { get { return owner.countdown; } }
  public BombStatus bombStatus { get { return owner.bombStatus; } }

  void OnEnable() {
    this.AddObserver(Render, Notifications.BOMB_STATUS_CHANGED, owner.tile);
    this.AddObserver(Render, Notifications.BOMB_COUNTDOWN, owner);
    this.AddObserver(UpdateDigit, Notifications.BOMB_COUNTDOWN, owner);
  }
  void OnDisable() {
    this.RemoveObserver(Render, Notifications.BOMB_STATUS_CHANGED, owner.tile);
    this.RemoveObserver(Render, Notifications.BOMB_COUNTDOWN, owner);
    this.RemoveObserver(UpdateDigit, Notifications.BOMB_COUNTDOWN, owner);
  }
  protected void Awake() {
    owner = GetComponent<TileEntity>();
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

  void Render(object sender = null, object e = null) {
    var coordinates = new Vector2(3, 3);

    switch (bombStatus) {
      case BombStatus.None:
        coordinates = new Vector2(3, 3);
        break;
      case BombStatus.Box:
        coordinates = new Vector2(0, countdown == 2 ? 1 : 2);
        break;
      case BombStatus.Star:
        coordinates = new Vector2(1, countdown == 2 ? 1 : 2);
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

    showSpark = countdown == 1;
    spark.gameObject.SetActive(showSpark);
    transform.localScale = Vector3.one;
  }
  void UpdateDigit(object sender = null, object e = null) {
    digit.UpdateDigit();
  }
}
