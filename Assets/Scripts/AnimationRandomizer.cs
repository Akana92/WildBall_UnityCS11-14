using UnityEngine;

public class AnimationRandomizer : MonoBehaviour
{
    private Animator animator;
    private int numberOfAnimations = 3;
    private int previousIndex = -1;

    void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogWarning("Animator не найден на объекте " + gameObject.name);
        }
        else
        {
            RandomizeAnimation();
        }
    }

    public void RandomizeAnimation()
    {
        if (animator == null)
            return;

        int randomIndex = Random.Range(0, numberOfAnimations);

        if (randomIndex == previousIndex)
        {
            animator.SetTrigger("RepeatAnimation");
            Debug.Log("Повторяем ту же анимацию на " + gameObject.name);
        }
        else
        {
            animator.SetInteger("AnimationIndex", randomIndex);
            Debug.Log("Переключаемся на новую анимацию на " + gameObject.name + ", randomIndex: " + randomIndex);
            previousIndex = randomIndex;
        }
    }
}
