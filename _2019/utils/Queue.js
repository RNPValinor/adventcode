class Queue {
  constructor() {
    this._data = [];
  }

  enqueue(item) {
    this._data.push(item);
  }

  dequeue() {
    return this._data.shift();
  }
}

module.exports = Queue;
