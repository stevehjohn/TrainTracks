# Train Tracks

An engine based around the Train Track puzzle games.

## Puzzles States

```
 #  | Size    | Time     | Iterations | Correct |
----+---------+----------+------------+---------+
 0  | (12x12) | ~1.5s    | ~17m       | Yes     |
 1  | (12x11) | ~0.2s    | ~700k      | Yes     |
 2  | (11x11) | ~0,2s    | ~650k      | Yes     |
 3  | (11x12) | ~1m      | ~106m      | Yes     |
 4  | (6x6)   | <0.01s   | ~100       | Yes     |
 5  | (6x6)   | <0.001s  | ~100       | Yes     |
 6  | (9x9)   | <0.4s    | ~600k      | Yes     |
 7  | (9x9)   | ~0.01s   | ~14k       | Yes     |
 8  | (10x10) | ~0.01s   | ~436       | Yes     |
 9  | (12x11) | ~5s      | ~8m        | Yes     |
 10 | (7x6)   | <0.01s   | ~1.5k      | Yes     |
 11 | (11x12) | ~1s      | ~1m        | Yes     |
 12 | (12x12) | ~9h 40m  | ~55b       | Yes     |
 13 | (10x7)  | <0.01s   | ~21k       | Yes     |
 14 | (6x10)  | <0.001s  | ~350       | Yes     |
 15 | (9x10)  | ~1s      | ~1.7m      | Yes     |
 16 | (10x8)  | ~4s      | ~8.5m      | Yes     |
 17 | (10x10) | ~3s      | ~5.3m      | Yes     |
```

## Ideas

- Prefill 2s, 3s and 4s on the edges where possible.
- Prefill Xs where possible.
- If a row is full with no dangling edges, leave it as is? Must be correct?