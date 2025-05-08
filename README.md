# Train Tracks

An engine based around the Train Track puzzle games.

## Puzzles States

```
 #  | Size    | Time     | Iterations | Correct |
----+---------+----------+------------+---------+
 0  | (12x12) | ~12m 20s | ~1.26b     | Yes     |
 1  | (12x11) | ~1m 30s  | ~161m      | Yes     |
 2  | (11x11) | ~7s      | ~14m       | Yes     |
 3  | (11x12) | ~1m      | ~106m      | Yes     |
 4  | (6x6)   | <0.001s  | ~350       | Yes     |
 5  | (6x6)   | <0.001s  | ~100       | Yes     |
 6  | (9x9)   | <0.5s    | ~800k      | Yes     |
 7  | (9x9)   | ~0.01s   | ~27k       | Yes     |
 8  | (10x10) | ~0.05s   | ~160k      | Yes     |
 9  | (12x11) | ~10s     | ~16m       | Yes     |
 10 | (7x6)   | <0.01s   | ~1.5k      | Yes     |
 11 | (11x12) | ~15s     | ~25m       | Yes     |
 12 | (12x12) | ~9h 40m  | ~55b       | Yes     |
 13 | (10x7)  | <0.1s    | ~25k       | Yes     |
 14 | (6x10)  | <0.001s  | ~350       | Yes     |
```

## Ideas

- Prefill 2s, 3s and 4s on the edges where possible.
- Prefill Xs where possible.
- If a row is full with no dangling edges, leave it as is? Must be correct?