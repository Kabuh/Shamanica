using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationsFX : MonoBehaviour
{
    public static AnimationsFX Instance;

    class SpriteObj{
        public GameObject myGameObject;
        public Hero myParentHero;
        public Vector3 startPos;
        public Vector3 endPos;
    }

    
    public float spriteAnimSpeed;

    private void Awake()
    {
        #region Singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance == this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        #endregion

    }

    public void AnimateSprite(GameObject spriteGO, Hero HeroObject, bool isAttack)
    {
        SpriteObj freshSprite = new SpriteObj();
        freshSprite.myGameObject = spriteGO;
        freshSprite.myParentHero = HeroObject;


        freshSprite.startPos = freshSprite.myGameObject.transform.localPosition;
        //due to change
        freshSprite.endPos = new Vector3(0, 0, 0.5f);

        IEnumerator coroutine = null;
        coroutine = SpriteMovement(freshSprite, isAttack);
        StartCoroutine(coroutine);
    }

    IEnumerator SpriteMovement(SpriteObj sprite, bool isAttack) {
        sprite.myGameObject.SetActive(true);

        while (sprite.endPos != sprite.myGameObject.transform.localPosition) {
            sprite.myGameObject.transform.localPosition = Vector3.MoveTowards(sprite.myGameObject.transform.localPosition, sprite.endPos, spriteAnimSpeed * Time.deltaTime);
            yield return null;
        }
                
        sprite.myParentHero.RefreshText();
        if (!isAttack) {
            sprite.myParentHero.TextBuff();
        }
        if (sprite.myParentHero.CurrentHP <= 0)
        {
            sprite.myParentHero.Death();
        }


        sprite.myGameObject.transform.localPosition = sprite.startPos;
        sprite.myGameObject.SetActive(false);
        TurnComponent.Instance.CheckForNewTurn();
    }
}
