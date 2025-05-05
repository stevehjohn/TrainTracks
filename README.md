# Train Tracks

An engine based around the Train Track puzzle games.

## Puzzles States

```
 #  | Size    | Time     | Iterations | Correct |
----+---------+----------|------------+---------+
 0  | (12x12) | ~13m 40s | ~1.4b      | Yes     |
 1  | (12x11) | ~1m 40s  | ~181m      | Yes     |
 2  | (11x11) | ~8s      | ~16m       | Yes     |
 3  | (11x12) | ~1m 6s   | ~121m      | Yes     |
 4  | (6x6)   | <0.001s  | ~400       | Yes     |
 5  | (6x6)   | <0.001s  | ~200       | Yes     |
 6  | (9x9)   | <0.5s    | ~1m        | Yes     |
 7  | (9x9)   | ~0.01s   | ~33k       | Yes     |
 8  | (10x10) | ~0.05s   | ~180k      | Yes     |
 9  | (12x11) | ~10s     | ~17m       | Yes     |
 10 | (7x6)   | <0.001s  | ~1.7k      | Yes     |
 11 | (11x12) | ~15s     | ~26m       | Yes     |
 12 | (12x12) |          |            |         |
```

## Ideas

- Prefill 2s, 3s and 4s on the edges where possible.