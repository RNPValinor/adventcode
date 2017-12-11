let input = 265149

let root = Math.floor(Math.sqrt(input))
let coords = {
    x: 0,
    y: 0
}

if (root % 2 === 0) {
    coords = {
        x: 1 - (root / 2),
        y: 0 - (root / 2)
    }
} else {
    let dist = Math.floor(root / 2)

    coords = {
        x: dist,
        y: dist
    }
}

let step = (coords) => {
    if (coords.x >= 0) {
        if (coords.y <= 0) {
            if (Math.abs(coords.y) < coords.x) {
                coords.y--
            } else {
                coords.x--
            }
        } else {
            if (coords.x <= coords.y) {
                coords.x++
            } else {
                coords.y--
            }
        }
    } else {
        if (coords.y <= 0) {
            if (coords.y < coords.x) {
                coords.x--
            } else {
                coords.y++
            }
        } else {
            if (coords.y < Math.abs(coords.x)) {
                coords.y++
            } else {
                coords.x++
            }
        }
    }
}

input -= Math.pow(root, 2)

while (input > 0) {
    step(coords)

    input--
}

console.log("(" + coords.x + ", " + coords.y + ")")
console.log("Distance: " + (Math.abs(coords.x) + Math.abs(coords.y)))

let n = 1
let grid = {
    0: {
        0: 1
    }
}

let sumSurrounding = (grid, coords) => {
    let sum = 0
    for (let i = -1; i <= 1; i++) {
        for (let j = -1; j <= 1; j++) {
            if (grid[coords.x + i]) {
                sum += grid[coords.x + i][coords.y + j] || 0
            }
        }
    }
    return sum
}

coords = {
    x: 1,
    y: 0
}

while (n < 265149) {
    if (!grid[coords.x]) {
        grid[coords.x] = {}
    }
    
    grid[coords.x][coords.y] = n

    step(coords)

    n = sumSurrounding(grid, coords)
}

console.log("Smallest value: " + n)
