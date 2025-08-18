using System;
using System.Collections.Generic;
using TrganReport.Enums;

namespace TrganReport.Models;

internal sealed class ExecutionDetails {
    private readonly object _lock = new();

    private string _name = null!;
    private string[] _category = null!;
    private DateTime _startTime;
    private DateTime _endTime;
    private string _duration = null!;
    private Status _status;

    public string Name {
        get {
            lock (_lock) {
                return _name;
            }
        }
        internal set {
            lock (_lock) {
                _name = value;
            }
        }
    }

    public string[] Category {
        get {
            lock (_lock) {
                return _category;
            }
        }
        internal set {
            lock (_lock) {
                _category = value;
            }
        }
    }

    public DateTime StartTime {
        get {
            lock (_lock) {
                return _startTime;
            }
        }
        internal set {
            lock (_lock) {
                _startTime = value;
            }
        }
    }

    public DateTime EndTime {
        get {
            lock (_lock) {
                return _endTime;
            }
        }
        internal set {
            lock (_lock) {
                _endTime = value;
            }
        }
    }

    public string Duration {
        get {
            lock (_lock) {
                return _duration;
            }
        }
        internal set {
            lock (_lock) {
                _duration = value;
            }
        }
    }

    public Status Status {
        get {
            lock (_lock) {
                return _status;
            }
        }
        internal set {
            lock (_lock) {
                _status = EscalateStatus(value, _status);
            }
        }
    }
    private static readonly Dictionary<Status, int> StatusPriority = new() {
        [Status.PASS] = 1,
        [Status.SKIP] = 2,
        //[Status.WARN] = 3,
        [Status.FAIL] = 4
    };
    private static Status EscalateStatus(Status newStatus, Status currentStatus) {
        int newPriority = StatusPriority.TryGetValue(newStatus, out int np) ? np : -1;
        int currentPriority = StatusPriority.TryGetValue(currentStatus, out int cp) ? cp : -1;
        return newPriority > currentPriority ? newStatus : currentStatus;
    }
}

