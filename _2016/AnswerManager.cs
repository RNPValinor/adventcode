using System;
using _2016.Answers;

namespace _2016
{
    public class AnswerManager
    {
        private Parts parts;
        private IAnswer answer;

        public AnswerManager(string day, string parts) {
            switch (parts) {
                case "1":
                    this.parts = Parts.First;
                    break;
                case "2":
                    this.parts = Parts.Second;
                    break;
                default:
                    this.parts = Parts.Both;
                    break;
            }

            var type = Type.GetType("_2016.Answers.Day" + day + "Answer");
            this.answer = (IAnswer)Activator.CreateInstance(type);
        }

        public void Run() {
            if (this.parts == Parts.First || this.parts == Parts.Both) {
                answer.PartOne();
            }

            if (this.parts == Parts.Second || this.parts == Parts.Both) {
                answer.PartTwo();
            }
        }
    }

    enum Parts {
        First, Second, Both
    }
}