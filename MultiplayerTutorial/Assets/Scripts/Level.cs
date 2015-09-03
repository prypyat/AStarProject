using UnityEngine;

public class Level : MonoBehaviour {

    public int[,] Nodes = new int[10,10];

    public GameObject Blocker;
    public GameObject Player;
    public Astar Astar;
    public AstarMap Map;
    private static Level _instance;

    void Awake() {

        _instance = this;

        for (int i = 0; i < Nodes.GetLength(0); i++) {

            for (int j = 0; j < Nodes.GetLength(1); j++) {
                Nodes[i, j] = Random.Range(0,3)<1?1:0;
            }

        }

    }

    void Start() {

        
        Map = new AstarMap(10,10);

        for (int i = 0; i < Nodes.GetLength(0); i++) {

            for (int j = 0; j < Nodes.GetLength(1); j++) {

                if (Nodes[i, j] == 1) {

                    Map.RegisterWalkability(new Vector2(i, j), false);

                    GameObject colon = Instantiate(Blocker) as GameObject;
                    colon.transform.SetParent(transform);
                    colon.transform.localPosition = Grid2World(Map.GetNode(i,j).Position);

                }
            }

        }

        Astar = new Astar(Map);

        CreatePlayer();
    }

    public void CreatePlayer(bool remote = false) {
        GameObject player = Instantiate(Player) as GameObject;

        bool finding = true;

        while (finding) {

            int rx = Random.Range(0, 11);
            int ry = Random.Range(0, 11);

            finding = !Map.GetNodeWalkability(new Vector2(rx, ry));

            if (!finding) {
                player.transform.position = Grid2World(new Vector2(rx, ry));
            }

        }

    }

    public Vector3 Grid2World(Vector2 position) {
        return new Vector3(-5 + position.x, 0, -5 + position.y);
    }

    public Vector2 World2Grid(Vector3 position) {
        return new Vector2(Mathf.FloorToInt(position.x+5),Mathf.FloorToInt(position.z+5));
    }

    public static Level Instance {
        get { return _instance; }
    }

}
