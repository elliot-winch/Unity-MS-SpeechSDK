using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpeechRecognitionService;
using Microsoft.Unity;

namespace FactualVR
{
    public class VoiceCommand
    {
        public string phrase;
        public Action command;
    }

    public class VoiceContext
    {
        public List<VoiceCommand> commands = new List<VoiceCommand>();
    }

    [RequireComponent(typeof(FactualSpeechManager))]
    public class VoiceCommandManager : MonoBehaviour
    {
        public VoiceContext Context { get; set; }

        private void Start()
        {
            var sm = GetComponent<FactualSpeechManager>();

            sm.MessageReceived += CallContext;

            //Testing
            Context = new VoiceContext()
            {
                commands = new List<VoiceCommand>()
                {
                    new VoiceCommand()
                    {
                        phrase = "Hello",
                        command = () =>
                        {
                            Debug.Log("Hello Command Called");
                        }
                    }
                }
            };
        }

        private void CallContext(SpeechServiceResult result)
        {
            Debug.Log("Voice Command Manager -> CallContext: " + result.Path);

            if (result.Path != SpeechServiceResult.SpeechMessagePaths.SpeechPhrase)
            {
                Debug.Log("Voice Command Manager -> CallContext: Incomplete message received");
                //Message is not complete
                return;
            }

            if (Context == null)
            {
                Debug.Log("Voice Command Manager -> CallContext: No context provided");
                //no context to assess
                return;
            }

            Debug.Log("Voice Command Manager -> CallContext: Text is " + result.Result.DisplayText);

            foreach (var c in Context.commands)
            {
                //TODO: equalilty testing - how close does the word need to be
                if (result.Result.DisplayText.ToLower().Equals(c.phrase))
                {
                    c.command?.Invoke();
                }
            }
        }
    }
}
