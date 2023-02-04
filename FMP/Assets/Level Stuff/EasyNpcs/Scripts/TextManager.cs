using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Npc_AI
{
    public class TextManager : MonoBehaviour
    {
        void Start()
        {
            FindDialogue.allPrefabs = Resources.LoadAll<DialoguePrefab>("Dialogues");
        }
    }

    public class FindDialogue : MonoBehaviour
    {
        public static DialoguePrefab[] allPrefabs;

        public static Tuple<List<string>, List<string>> GetDialgoue(Gender[] genders = null, Job[] jobs = null)
        {
            genders = genders ?? new Gender[] { Gender.Default, Gender.Default };
            jobs = jobs ?? new Job[] { Job.Default, Job.Default };

            List<DialoguePrefab> validTexts = Parse_DialogueTexts(genders, jobs);

            if (validTexts.Count < 1)
                return null;

            System.Random random = new System.Random();
            var npc = validTexts[random.Next(0, validTexts.Count - 1)];

            return new Tuple<List<string>, List<string>>(npc.npc1, npc.npc2);
        }

        static List<DialoguePrefab> Parse_DialogueTexts(Gender[] genders, Job[] jobs)
        {
            List<DialoguePrefab> validTexts = new List<DialoguePrefab>();
            foreach (var text in allPrefabs)
            {
                bool isValid = true;
                if (!jobs[0].HasFlag(text.jobNpc1) || !genders[0].HasFlag(text.genderNpc1))
                {
                    isValid = false;
                }
                if (!jobs[1].HasFlag(text.jobNpc2) || !genders[1].HasFlag(text.genderNpc2))
                {
                    isValid = false;
                }

                if (isValid)
                {
                    validTexts.Add(text);
                }
            }

            return validTexts;
        }
    }
}