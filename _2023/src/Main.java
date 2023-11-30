import org.apache.commons.cli.*;
import solvers.Solvers;

public class Main {
    public static void main(String[] args) {
        var options = new Options()
                .addOption(Option.builder("d")
                        .longOpt("day")
                        .hasArg(true)
                        .desc("Which day to solve")
                        .argName("day")
                        .build());

        var parser = new DefaultParser();

        try
        {
            var cmdLine = parser.parse(options, args);
            var dayStr = cmdLine.getOptionValue('d');

            var day = Integer.parseInt(dayStr);

            var solver = Solvers.GetSolver(day);

            solver.Solve();
        }
        catch (ParseException e)
        {
            new HelpFormatter().printHelp("apache args...", options);
        }
    }
}