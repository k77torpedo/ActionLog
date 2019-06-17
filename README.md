# ActionLog (Beta Release)

## What is it?
Records actions or events in games into a continuous 'Log' on a frame-by-frame basis using RingBuffers only.

## When to use it:
* You want to log certain actions or events for display.
* You want different systems to access actions or events without relying on C#-events or callbacks. 

In our games we sometimes don't want to calculate everything in one frame but rather just _log_ that something has happened to then later let other systems simply look it up and process it. A good example would be if a player takes a hit from a monster: Different Effects or Skills might want to react to _"Player is taking 5 damage"_ but doing all that on the same frame might lead to lag or stutter in which case the `ActionLog` might be able to help you.

## How to ...?

### Create a new ActionLog
```
        int frameCapacity = 10;
        int actionCapacity = 5;
        int actionBufferCapacity = 10;

        ActionLogFloat actionLog = new ActionLogFloat(frameCapacity, actionCapacity, actionBufferCapacity);
```
Explanation: 
* With a _frameCapacity_ of 10 we can keep track of up to 10 frames before the oldest one will be recycled. 
* With an _actionCapacity_ of 5 we can log 5 different actions or events for every frame. 
* With an _actionBufferCapacity_ of 10 we can make 10 entries for each(!!!) action before the oldest on will be recycled.

Note: The _actionCapacity_ also dictates the _actionIds_ for the actions you want to add. An _actionCapacity_ of 7 will give you the _actionIds_ 0, 1, 2, 3, 4, 5 and 6. An _actionCapacity_ of 4 will give you the _actionIds_ 0, 1, 2 and 3.

### Add a new frame to the ActionLog
```
        ulong newFrame = 9999L;
        actionLog.AddFrame(newFrame);
```
This will add a new frame to the ActionLog.

### Add an action to a frame

#### Add an action to the current frame
```
        int actionId = 2;
        float actionData = 15f;
        actionLog.AddAction(actionId, actionData);
```
This will add the action to the current frame.

#### Add an action retrospectively to a previous frame
```
        ulong firstFrame = 9998L;
        actionLog.AddFrame(firstFrame);
        
        ulong secondFrame = 9999L;
        actionLog.AddFrame(secondFrame);
        
        int actionId = 2;
        float actionData = 15f;
        actionLog.AddAction(firstFrame, actionId, actionData);
```
This will add the action to the specified frame.

#### Add an action to the current frame or create a new frame automatically
```
        ulong frame = 9999L;
        int actionId = 2;
        float actionData = 15f;
        actionLog.Add(frame, actionId, actionData);
```
If the specified frame is the current frame the action will be added to it, else a new frame with the action will automatically be added.

This is the preferred way of adding to the `ActionLogFloat`.

### Read logged actions

#### Method 1
```
        int actionId = 2:
        actionLog.AddAction(actionId, 22f);
        actionLog.AddAction(actionId, 44f);
        actionLog.AddAction(actionId, 66f);
        
        RingBufferFloat buffer = actionLog.GetActionsBuffer(actionId);
        for (int i = 0; i < buffer.Count; i++) {
            Debug.Log("Recorded action data: " + buffer[i]);
        }
```
By receiving a shallow copy of the `RingBufferFloat` we can simply iterate through its collection to receive all actions that have been added.

#### Method 2
```
        int actionId = 2:
        actionLog.AddAction(actionId, 22f);
        actionLog.AddAction(actionId, 44f);
        actionLog.AddAction(actionId, 66f);

        int[] index = actionLog.GetActionsIndex(actionId);
        for (int i = 0; i < actionLog.Actions[index[0], index[1]].Count; i++) {
            Debug.Log("Recorded action data: " + actionLog.Actions[index[0], index[1]][i]);
        }
```
By receiving the multidimensional index of where the `RingBufferFloat` is located inside the `ActionLogFloat.Actions` we can directly access it and do not need to produce any shallow copies of it.

## Closure
Currently supported types:
* `ActionLogByte`
* `ActionLogByteArray`
* `ActionLogShort`
* `ActionLogShortArray`
* `ActionLogInt`
* `ActionLogIntArray`
* `ActionLogFloat`
* `ActionLogFloatArray`


# MIT License
Copyright (c) 2019 k77torpedo

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
