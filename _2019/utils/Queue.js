class Queue {
  constructor() {
    this._data = [];
    this._index = 0;
  }

  enqueue(item) {
    this._data.push(item);
  }

  dequeue() {
    return this._data[this._index++];
  }

  hasMore() {
    return this._index < this._data.length;
  }
}

module.exports = Queue;
