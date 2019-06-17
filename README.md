# ActionLog

## What is it?
Records actions or events in games with their respective data into a continuous 'Log' on a frame-by-frame basis using RingBuffers only.

## When to use it:
* You want to log certain action or events for display.
* You want different systems to access logged actions without relying on events or callbacks. 

The latter is especially important in games since sometimes we don't want to calculate everything in one frame but rather just _log_ that something happened to then later let other systems simply look it up and process it. A good example would be if a player takes a hit from a monster: Different Effects or Skills might want to react to _"Player is taking a hit"_ but doing all that on the same frame might lead to lag or stutter in which case the `ActionLog` can help you.

## How does it work?
The `ActionLog` maintains a RingBuffer for all added frames as well as arrays of RingBuffers for the respective actions and their stored data. 

//Picture of data organization

Organizing data this way keeps performance relatively fast and prevents common problems like lists or dictionaries becoming too long. Adding as well as accessing the data also remains fast.

## How to ...?

### Create a new log
```
        int frameCapacity = 10;
        int actionCapacity = 5;
        int actionBufferCapacity = 10;

        ActionLog log = new ActionLog(frameCapacity, actionCapacity, actionBufferCapacity);
```
Explanation: 
* With a _frameCapacity_ of 10 we can keep track of up to 10 frames before the oldest one will be recycled. 
* With an _actionCapacity_ of 5 we can log 5 different actions or events for every frame. 
* With an _actionBufferCapacity_ of 10 we can make 10 entries for each(!!!) action before the oldest on will be recycled.

### Add a new frame to the log

### Add an action to a frame


### Read logged actions



