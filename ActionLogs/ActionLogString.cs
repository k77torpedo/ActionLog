﻿namespace ActionLog {
    /// <summary>
    /// Records actions with their respective data into a continuous 'Log' on a frame-by-frame basis. 
    /// </summary>
    public class ActionLogString {
        //Fields
        private int                         _frameCapacity;
        private int                         _actionCapacity;
        private int                         _actionBufferCapacity;
        private RingBufferUlong             _frames;
        private RingBufferString[,]         _actions;

        public int                          FrameCapacity                   { get { return _frameCapacity; } }
        public int                          ActionCapacity                  { get { return _actionCapacity; } }
        public int                          ActionBufferCapacity            { get { return _actionBufferCapacity; } }
        public RingBufferUlong              Frames                          { get { return _frames; } }
        public RingBufferString[,]          Actions                         { get { return _actions; } }
        public ulong                        CurrentFrame                    { get { return _frames[_frames.Head]; } }


        //Functions
        /// <summary>
        /// Creates a new instance of the <see cref="ActionLogString"/>-class.
        /// </summary>
        /// <param name="pFrameCapacity">The maximum amount of frames this log is able to track before the oldest will be overwritten.</param>
        /// <param name="pActionCapacity">The amount of actions this log is able to track.</param>
        /// <param name="pActionBufferCapacity">The amount of entries that can be tracked per action and frame before the oldest will be overwritten.</param>
        public ActionLogString (int pFrameCapacity, int pActionCapacity, int pActionBufferCapacity) {
            _frameCapacity = pFrameCapacity;
            _actionCapacity = pActionCapacity;
            _actionBufferCapacity = pActionBufferCapacity;
            _frames.Init(pFrameCapacity);
            _actions = new RingBufferString[pFrameCapacity, pActionCapacity];
        }

        /// <summary>
        /// If the specified frame is the current frame the action will be added to it, else a new frame with the action will be added.
        /// </summary>
        /// <param name="pFrame">The frame.</param>
        /// <param name="pActionId">The id of the action.</param>
        /// <param name="pActionData">The data of the action.</param>
        public void Add (ulong pFrame, int pActionId, string pActionData) {
            if (pFrame != _frames[_frames.Head]) {
                _frames.Add(pFrame);
                for (int i = 0; i < _actionCapacity; i++) {
                    if (_actions[_frames.Head, i].Capacity != _actionBufferCapacity) {
                        _actions[_frames.Head, i].Init(_actionBufferCapacity);
                    } else {
                        _actions[_frames.Head, i].Clear();
                    }
                }
            }

            _actions[_frames.Head, pActionId].Add(pActionData);
        }

        /// <summary>
        /// Adds a new frame to the log.
        /// </summary>
        /// <param name="pFrame">The frame.</param>
        public void AddFrame (ulong pFrame) {
            _frames.Add(pFrame);
            for (int i = 0; i < _actionCapacity; i++) {
                if (_actions[_frames.Head, i].Capacity != _actionBufferCapacity) {
                    _actions[_frames.Head, i].Init(_actionBufferCapacity);
                } else {
                    _actions[_frames.Head, i].Clear();
                }
            }
        }

        /// <summary>
        /// Add an action to the current frame.
        /// </summary>
        /// <param name="pActionId">The id of the action.</param>
        /// <param name="pActionData">The data of the action.</param>
        public void AddAction (int pActionId, string pActionData) {
            _actions[_frames.Head, pActionId].Add(pActionData);
        }

        /// <summary>
        /// Add an action to the specified frame.
        /// </summary>
        /// <param name="pFrame">The frame.</param>
        /// <param name="pActionId">The id of the action.</param>
        /// <param name="pActionData">The data of the action.</param>
        public void AddAction (ulong pFrame, int pActionId, string pActionData) {
            for (int i = 0; i < _frames.Count; i++) {
                if (_frames[i] == pFrame) {
                    _actions[_frames.TranslateIndex(i), pActionId].Add(pActionData);
                    return;
                }
            }
        }

        /// <summary>
        /// Gets a shallow copy of the buffer for the specified actionId of the current frame. Intended for read-only operations.
        /// </summary>
        /// <param name="pActionId">The id of the action.</param>
        /// <returns>The buffer.</returns>
        public RingBufferString GetActionsBuffer (int pActionId) {
            return _actions[_frames.Head, pActionId];
        }

        /// <summary>
        /// Gets a shallow copy of the buffer for the specified actionId of the specified frame. Intended for read-only operations.
        /// </summary>
        /// <param name="pFrame">The frame.</param>
        /// <param name="pActionId">The id of the action.</param>
        /// <returns>The buffer if the frame was found, otherwise an uninitialized buffer.</returns>
        public RingBufferString GetActionsBuffer (ulong pFrame, int pActionId) {
            for (int i = 0; i < _frames.Count; i++) {
                if (_frames[i] == pFrame) {
                    return _actions[_frames.TranslateIndex(i), pActionId];
                }
            }

            return default;
        }

        /// <summary>
        /// Tries to get a shallow copy of the buffer for the specified actionId of the specified frame. Intended for read-only operations.
        /// </summary>
        /// <param name="pFrame">The frame.</param>
        /// <param name="pActionId">The id of the action.</param>
        /// <param name="pBuffer">The buffer.</param>
        /// <returns>True if the frame was found, otherwise false.</returns>
        public bool TryGetActionsBuffer (ulong pFrame, int pActionId, out RingBufferString pBuffer) {
            for (int i = 0; i < _frames.Count; i++) {
                if (_frames[i] == pFrame) {
                    pBuffer = _actions[_frames.TranslateIndex(i), pActionId];
                    return true;
                }
            }

            pBuffer = default;
            return false;
        }

        /// <summary>
        /// Determines if the specified frame currently exists.
        /// </summary>
        /// <param name="pFrame">The frame.</param>
        /// <returns>True if the frame exists, otherwise false.</returns>
        public bool ContainsFrame (ulong pFrame) {
            for (int i = 0; i < _frames.Count; i++) {
                if (_frames[i] == pFrame) {
                    return true;
                }
            }

            return false;
        }
    }
}
