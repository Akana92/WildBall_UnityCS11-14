using UnityEngine;

public class Gem : MonoBehaviour
{
    private Animator animator;
    private bool isCollected = false;

    void Start()
    {
        animator = GetComponent<Animator>();

        if (animator == null)
        {
            Debug.LogWarning("Animator �� ������ �� ������� " + gameObject.name);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!isCollected && other.CompareTag("Player"))
        {
            // ��������� �������� ������������
            animator.SetTrigger("Collected");
            isCollected = true;

            // ��������� ���������, ����� ������������� ��������� ������������
            GetComponent<Collider>().enabled = false;

            // ��������� �������� ��� ����������� ������� ����� ��������
            StartCoroutine(DestroyAfterAnimation());
        }
    }

    System.Collections.IEnumerator DestroyAfterAnimation()
    {
        // ��� ������������ �������� ������������
        float disappearAnimationLength = 1.0f; // ������� �������� ������������
        yield return new WaitForSeconds(disappearAnimationLength);

        // ���������� ������������ ������ ������ � ��������
        Destroy(transform.parent.gameObject);
    }
}
