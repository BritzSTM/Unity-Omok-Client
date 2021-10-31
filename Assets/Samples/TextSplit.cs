using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextSplit : MonoBehaviour
{
    public TMP_Text _text;
    public int FontSize = 36;
    private void Awake()
    {
        var font = _text.font;
        var prev = DateTime.Now;
        var chs = "가나다마바사 아차카파마하마아 길드의 공지 입니다. ㅇㅅㅇㅅㅇㅅㅇㅅㅇㅅ";
        //"Some Message Some Message Some Message Some Message";
        Debug.Log(_text.GetPreferredValues(_text.text));
        Debug.Log(TextWidthApproximation(chs, _text.font, (int)_text.fontSize, FontStyles.Normal));
        Debug.Log((DateTime.Now - prev).Milliseconds);
    }

    public float TextWidthApproximation(string text, TMP_FontAsset fontAsset, int fontSize, FontStyles style)
    {
        Debug.Log(fontAsset.TryAddCharacters("가"));
        // Compute scale of the target point size relative to the sampling point size of the font asset.
        float pointSizeScale = fontSize / (fontAsset.faceInfo.pointSize * fontAsset.faceInfo.scale);
        float emScale = FontSize * 0.01f;

        float styleSpacingAdjustment = (style & FontStyles.Bold) == FontStyles.Bold ? fontAsset.boldSpacing : 0;
        float normalSpacingAdjustment = fontAsset.normalSpacingOffset;
        
        float width = 0;
        for (int i = 0; i < text.Length; i++)
        {
            char unicode = text[i];
            TMP_Character character;
            // Make sure the given unicode exists in the font asset.
            if (fontAsset.characterLookupTable.TryGetValue(unicode, out character))
                width += character.glyph.metrics.horizontalAdvance * pointSizeScale + (styleSpacingAdjustment + normalSpacingAdjustment) * emScale;
        }

        return width;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
