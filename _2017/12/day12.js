var fs = require('fs')
var _ = require('lodash')

var graph = {}
var ungroupedNodes = []

var dfs = (start) => {
    let subgraph = {}
    let toExplore = [start]

    while (toExplore.length) {
        let node = toExplore.pop()
        subgraph[node] = true
        ungroupedNodes.splice(ungroupedNodes.indexOf(node), 1)

        let links = graph[node]

        for (let i = 0; i < links.length; i++) {
            if (!subgraph[links[i]]) {
                toExplore.unshift(links[i])
            }
        }
    }

    return subgraph
}

fs.readFile('pipes.txt', (err, data) => {
    let lines = data.toString().split('\r\n')
    
    for (let i = 0; i < lines.length; i++) {
        let line = lines[i].split(' <-> ')

        let source = parseInt(line[0])
        let targets = line[1].split(', ')

        graph[source] = []
        ungroupedNodes.push(source)

        for (let j = 0; j < targets.length; j++) {
            graph[source].push(parseInt(targets[j]))
        }
    }

    let numGroups = 0

    while (ungroupedNodes.length) {
        let node = ungroupedNodes[0]
        dfs(node)
        numGroups++
    }

    let subgraph = dfs(0)

    console.log('Num programs in group: ' + _.size(subgraph))
    console.log('Total num groups: ' + numGroups)
})