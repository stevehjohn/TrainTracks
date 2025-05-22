using System.Collections.Generic;
using System.Linq;

namespace TrainTracks.Desktop.Infrastructure;

public class FrameCounter
{
    private const int MaximumSamples = 100;

    private long TotalFrames { get; set; }
    
    private float TotalSeconds { get; set; }
    
    private float CurrentFramesPerSecond { get; set; }

    public float AverageFramesPerSecond { get; private set; }

    private readonly Queue<float> _sampleBuffer = new();

    public void Update(float deltaTime)
    {
        CurrentFramesPerSecond = 1.0f / deltaTime;

        _sampleBuffer.Enqueue(CurrentFramesPerSecond);

        if (_sampleBuffer.Count > MaximumSamples)
        {
            _sampleBuffer.Dequeue();
            
            AverageFramesPerSecond = _sampleBuffer.Average(i => i);
        }
        else
        {
            AverageFramesPerSecond = CurrentFramesPerSecond;
        }

        TotalFrames++;
        
        TotalSeconds += deltaTime;
    }
}