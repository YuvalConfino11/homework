using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Sequences
{

    [CreateAssetMenu(menuName = "Scriptable Objects/SequenceScriptable", fileName = "IntroSequences")]
    public class SequenceScriptable : ScriptableObject
    {
       
        public IntroSequence[] introSequences;
    }

    [System.Serializable]
    public class IntroSequence
    {

        [TextArea]
        public string m_MainText;



        
    }
}
