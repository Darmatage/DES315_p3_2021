using UnityEngine;

public class Hammer : MonoBehaviour
{
    private AudioSource _source;
    private Animator _animator;
    string button1;
    // Start is called before the first frame update
    void Start()
    {
        _source = GetComponent<AudioSource>();
        _animator = GetComponent<Animator>();
        button1 = gameObject.transform.parent.GetComponent<playerParent>().action1Input;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown(button1) && _animator.GetCurrentAnimatorStateInfo(0).IsName("Default"))
        {
            _animator.Play("Hammer");
        }
    }

    public void PlayHammerSound()
    {
        _source.Play();
    }
}
