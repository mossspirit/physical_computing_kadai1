using UnityEngine;
using System.Collections;

public class DontShowScreenReset : MonoBehaviour {

    public float speed = 0;
    public int spriteCount = 3;
    public bool game = true;
    [SerializeField] PlayerManager playerManager;

    void Update() {
        // 左へ移動
        Invoke("DelayMethod", 2.0f);
    }

    void OnBecameInvisible() {
        // スプライトの幅を取得
        float width = GetComponent<SpriteRenderer>().bounds.size.x;
        // 幅ｘ個数分だけ右へ移動
        transform.position += Vector3.right * width * spriteCount;
    }
    void DelayMethod() {
        if (game) transform.position += Vector3.left * speed * Time.deltaTime;
        playerManager.scrollCount(speed);
    }
}