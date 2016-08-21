using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public GameObject worldManag;
    private InitializeWorld initWorld;

    void Awake()
    {
        initWorld = worldManag.GetComponent<InitializeWorld>();
    }

	void Start () {
        initWorld.InitializeList();
        initWorld.SetupGround();
        initWorld.LayoutObjectAtRandom(worldManag.GetComponent<InitializeWorld>().treeObjects, Types.TREE);
        initWorld.LayoutObjectAtRandom(worldManag.GetComponent<InitializeWorld>().rockObjects, Types.ROCK);
        initWorld.LayoutObjectAtRandom(worldManag.GetComponent<InitializeWorld>().grassObjects, Types.GRASS);
    }
}
