
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using TMPro;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class TextMeshPro_Global : UdonSharpBehaviour
{
    private TMP_InputField inputField;
    [UdonSynced] private string currentText = "";

    void Start()
    {
        // TextMeshPro InputField コンポーネントを取得
        inputField = GetComponent<TMP_InputField>();
    }
    void Update()
    {
        if (currentText != inputField.text)
        {
            // 自分自身をオブジェクトのオーナーにする
            Networking.SetOwner(Networking.LocalPlayer, gameObject);

            // 同期データ 更新
            currentText = inputField.text;

            // 同期データ 送信
            RequestSerialization();
        }
    }

    // 同期データ 受信
    public override void OnDeserialization()
    {
        inputField.text = currentText;
        Debug.Log($"InputField の値が変更されました: {inputField.text}");
    }

    // プレイヤーが参加したら
    public override void OnPlayerJoined(VRCPlayerApi player)
    {
        // 後から参加したプレイヤーのために 同期データ 送信
        if (Networking.IsOwner(gameObject)) RequestSerialization();
    }
}
