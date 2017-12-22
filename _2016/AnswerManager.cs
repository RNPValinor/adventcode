namespace _2016
{
    public class AnswerManager
    {
        private Parts parts;
        private IAnswer answer;

        public AnswerManager(string day, string parts) {
            switch (parts) {
                case "1":
                    parts = Parts.First;
                    break
                case "2":
                    parts = Parts.Second;
                    break
                default:
                    parts = Parts.Both
                    break
            }

            switch (day) {
                default:
                    throw new Error("Invalid day: " + day)
            }
        }

        public void Run() {
            if (answer.First || answer.Both) {
                answer.PartOne();
            }

            if (answer.Second || answer.Both) {
                answer.PartTwo();
            }
        }
    }

    enum Parts {
        First, Second, Both
    }
}