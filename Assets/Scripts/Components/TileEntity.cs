using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileEntity : MonoBehaviour {
  private const float SPARK_ROTATE_SPEED = 60.0f;

  [SerializeField] private GameObject spark;

  public enum RenderType { Tile, Wall }

  [SerializeField] RenderType renderType;
  private Tile owner;
  private bool showSpark;

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
}
