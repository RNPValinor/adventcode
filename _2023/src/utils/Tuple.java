package utils;

import java.util.Objects;

public class Tuple<T, V> {
    private final T _first;

    private final V _second;

    public Tuple(T first, V second) {
        this._first = first;
        this._second = second;
    }

    public T getFirst() {
        return this._first;
    }

    public V getSecond() {
        return this._second;
    }

    @Override
    public boolean equals(Object o) {
        if (this == o) return true;
        if (o == null || getClass() != o.getClass()) return false;
        Tuple<?, ?> tuple = (Tuple<?, ?>) o;
        return Objects.equals(_first, tuple._first) && Objects.equals(_second, tuple._second);
    }

    @Override
    public int hashCode() {
        return Objects.hash(_first, _second);
    }
}
