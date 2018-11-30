let genA = {
  val: 883,
  factor: 16807,
  stack: []
}

let genB = {
  val: 879,
  factor: 48271,
  stack: []
}

let div = 2147483647

let runGenerator = (gen, div, factor, numPairs) => {
  while (gen.stack.length < numPairs) {
    gen.val = (gen.val * gen.factor) % div

    if (gen.val % factor === 0) {
      gen.stack.push(gen.val.toString(2).substr(-16))
    }
  }
}

runGenerator(genA, div, 4, 5000000)

console.log('Finished generating genA pair values')

runGenerator(genB, div, 8, 5000000)

console.log('Finished generating genB pair values')

let count = 0

for (let i = 0; i < genA.stack.length; i++) {
  if (genA.stack[i] === genB.stack[i]) {
    count++
  }
}

console.log('Final count: ' + count)
