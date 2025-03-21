using UnityEngine;


[RequireComponent(typeof(Animator))]

public class Equipableitem : MonoBehaviour
{

    public Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
       
        if (Input.GetMouseButtonDown(0) &&
            InventorySystem.Instance.isOpen == false &&
            CraftingSystem.Instance.isOpen == false  &&
            SelectionManager.Instance.handIsVisible == false 
            )
        {



            animator.SetTrigger("hit");
        }
    }

    private void GitHit()
    {
        GameObject selectedTree = SelectionManager.Instance.selectedTree;

        if (selectedTree != null)
        {
            selectedTree.GetComponent<ChoppableTree>().GetHit();
        }
    }
}
