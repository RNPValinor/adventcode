package solvers;

import utils.Maths;

import java.util.*;

@SuppressWarnings("unused")
public class Day8Solver extends BaseSolver {
    private final HashMap<String, String[]> _map = new HashMap<>();
    private final List<Integer> _instructions = new ArrayList<>();

    public Day8Solver() {
        super(8);
    }

    @Override
    protected void processLine(String line) {
        if (this._instructions.isEmpty()) {
            for (var i = 0; i < line.length(); i++) {
                switch (line.charAt(i)) {
                    case 'L':
                        this._instructions.add(0);
                        break;
                    case 'R':
                        this._instructions.add(1);
                        break;
                }
            }
        } else if (!line.isEmpty()) {
            var node = line.substring(0, 3);
            var left = line.substring(7, 10);
            var right = line.substring(12, 15);

            this._map.put(node, new String[]{left, right});
        }
    }

    @Override
    protected String solvePart1() {
        var instructionIndex = 0;
        var curPos = "AAA";
        var numSteps = 0;

        while (!curPos.equals("ZZZ")) {
            var instruction = this._instructions.get(instructionIndex);

            curPos = this._map.get(curPos)[instruction];
            numSteps++;

            instructionIndex = (instructionIndex + 1) % this._instructions.size();
        }

        return String.valueOf(numSteps);
    }

    @Override
    protected String solvePart2() {
        var numSteps = 1L;

        var nodes = this._map
                .keySet()
                .stream()
                .filter(n -> n.charAt(2) == 'A')
                .toArray(String[]::new);

        var found = 0;
        var stepIdx = 0;

        while (nodes.length > 0) {
            var instruction = this._instructions.get(stepIdx++ % this._instructions.size());

            for (int i = 0; i < nodes.length; i++) {
                nodes[i] = this._map.get(nodes[i])[instruction];
            }

            var nextNodes = new ArrayList<String>(nodes.length);

            for (String node : nodes) {
                if (node.charAt(2) == 'Z') {
                    numSteps = Maths.lcm(numSteps, stepIdx);
                } else {
                    nextNodes.add(node);
                }
            }

            nodes = nextNodes.toArray(String[]::new);
        }

        return String.valueOf(numSteps);
    }
}
