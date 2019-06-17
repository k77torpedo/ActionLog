/// <summary>
/// A forward 'FILO'-Ringbuffer storing <see cref="ulong"/> as data. The index '0' always is the last item added.
/// </summary>
public struct RingBufferUlong {
    //Fields
    private ulong[]             _buffer;
    private int                 _capacity;
    private int                 _count;
    private int                 _head;

    public int                  Capacity                { get { return _capacity; } }
    public int                  Count                   { get { return _count; } }
    public int                  Head                    { get { return _head; } }
    public bool                 IsInitialized           { get { return _buffer != null; } }

    public ulong this[int i] { 
        get {
            if (_head - i < 0) {
                return _buffer[_head - i + _capacity];
            } else {
                return _buffer[_head - i];
            }
        }
    }


    //Functions
    /// <summary>
    /// Initializes the buffer according to capacity.
    /// </summary>
    /// <param name="pCapacity">The capacity of the buffer.</param>
    public void Init (int pCapacity) {
        _capacity = pCapacity;
        _buffer = new ulong[pCapacity];
    }

    /// <summary>
    /// Adds the data to the next slot in the buffer.
    /// </summary>
    /// <param name="pData">The data.</param>
    public void Add (ulong pData) {
        _head = (_head + 1) % _capacity;
        _buffer[_head] = pData;
        if (_count < _capacity) {
            _count++;
        }
    }

    /// <summary>
    /// Translates an external index used to access elements in the buffer to the respective internal index.
    /// </summary>
    /// <param name="pIndex">The index to translate.</param>
    /// <returns>The internal index of the buffer.</returns>
    public int TranslateIndex(int pIndex) {
        if (_head - pIndex < 0) {
            return _head - pIndex + _capacity;
        } else {
            return _head - pIndex;
        }
    }

    /// <summary>
    /// Clears the buffer.
    /// </summary>
    public void Clear () {
        _head = _capacity - 1;
        _buffer[_head] = 0;
        _count = 0;
    }
}
