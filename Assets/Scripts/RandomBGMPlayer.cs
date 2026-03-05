using UnityEngine;

public class RandomBGMPlayer : MonoBehaviour
{
    [Header("Setting")]
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip[] _bgmList; // Mảng chứa danh sách nhạc

    private void Start()
    {
        // Kiểm tra xem danh sách có bài nhạc nào không
        if (_bgmList.Length > 0 && _audioSource != null)
        {
            // Chọn một con số ngẫu nhiên từ 0 đến tổng số bài hát
            int randomIndex = Random.Range(0, _bgmList.Length);

            // Gắn bài hát được chọn vào máy phát và bật lên
            _audioSource.clip = _bgmList[randomIndex];
            _audioSource.Play();
        }
    }
}