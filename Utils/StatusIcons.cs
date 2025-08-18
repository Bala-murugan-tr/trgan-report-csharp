using System.Collections.Generic;
using TrganReport.Enums;

namespace TrganReport.Utils;
internal static class StatusIcons {

    private static readonly Dictionary<Status, string> StatusIcon = new() {
        [Status.FAIL] = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABgAAAAYCAYAAADgdz34AAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAKdSURBVEhLtVbPbxJBFJ6DlM2wU432YKLlfzDRRC9ePRiPKo3+G169GmOMMY0He2y8WBOjB0lMW5ZlWaCQFLQcjHR3+bEU+sNYtNDYy/jeMG0JLJSifMkXhp2Z79t5vHkPchImzI0gTdVDatx9ouqleUEYU7MeOgdzctnpoUad6zRRewdiTZbd5Sz3i7NV+ETiGJ6pBswla+8DsfJNuW0IPF7wBeLuC5aqg0gDRCoczLwJc8I8vc2p6b4mbxOKVPGGqmXPUqOis3wLNpdBxO4V7SGsiZU4W2ty1aymaHh1Ssp1YSHvo3rJYF/3PESOyYC+iM39mi3GR3P4/ctvDuFKk4jjl6rHUCPWS/HmnZs8iMJX0i6/aJT4GTDqMQENGll/JWXbUKLWVbayKcPSscGDZNHijwo/uLm7zxUw6DGJlTlb2eJMt65JeUKoZn2YzDXEBrJkDWa4wGfyWxwR3ml6mMAYkoNqzkch7tcql9Ro8Y8CzkGzxO+tbQre7cM7uRqfrTSEASK80+o1wcyLFQ/8i9Y0oVFnBlMN3+42bB4FXidBTYjMA0jN4jO8OGhwKzuaAeLQBDMMTVAzELWfg4H9Bm/nvxogYj/3+Xnd4QpmE2ii9vgN8BjjChGGH1K0+HB8P7ITIhPLdhBqycF/T1Md0lQvXBZ3AWL1aRIcMXZ4koEc9qJF7PZFQ4DbDZbZFlWxvag/hy4VSx2lAkGX1+dEsYNTdAp2c6RiJ5AoKwGjnMGSO8gEBQeX66J3uUbQ8Pcp6GaZ0RpOiwdMN021bxekXB98zlE1Xp1VkzXxY6nxAS0T5g5bJjPdOTKfo1LlZICAbPqVPSGCjR4bPhLH8EzMJTZO2fS74Deq0zRZuw+5/fTobwuM8RnOyWV9QMhfoclFsUIQfbMAAAAASUVORK5CYII=",
        [Status.SKIP] = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABgAAAAYCAYAAADgdz34AAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAANCSURBVEhLlZa7SxxRFMZtQiQo62Pxse6qs+Or0EJNfBS+ShEsBNmExH9AC8FSLOwkhBBSJGglBhFBEFOIFhaKIMFgYSeoiIpW2mhSKDJfzjf3zDq7GTX54DA7c+f8zrnn3ntmM56SU1ZW6sTjCdj2hFy/0fR3gmP62v9LAC2ObS+I/RYgUFYGVFQAlZVJ45hjWYtONNqubk8LNTXPxPGTC6Pl5QHFxUBDgwFbFhCPIxm0pASIRuGEQl+d5uZMxQRLHEKS+QaqqoDychNgZATY2gIuLoCWFgMknNfCQmBmxoz39sKJxX44hYVhxaXKzdyyNt0smSEBBPrV0wMUFZkZxWLA8rIOiE5PIQxIubZlRs8Vey+Bf3YzJ5zGGZSWAnt7ShANDwOZmaY0Gxv6UHRyAnR1AZEIyBDWF8UaOeXlL91yeHDPcnKAuTmliCYngawsYHNTH4gOD4HGRrNWLB39hCVBXine3TFLydL4jU6Dg0oS7e4CKyt6Izo6Moufn38Pp3GHxePfDdy2S+TmJgXsmewO1NcD19dK9OnsDGhq+huuJsxb2XEx1v5NYPaesUzz80pVXV4CnZ2pZUk3zsKy3rI8Hx4MwD3Pmk9PK1l0cwP09QGh0MNwminTR85g9sEA2dnA2JiSVXd3QH+/KU2Qj2dmBrPBAZgZMxwYUGqadnaAggKzlf1+fksGkGmkBCA8HAY6OoCrKyWKVleBtTW9EQ0NPV4mBpDy8wy8SwnAw8Izsb+vJNHxsTl0ra3A7a15dn4OVFebE+8He2YCvPba8a370Du9zNYTF7W729ScGU9M6IBoYcG0Da8B+oxMsag5C5a17M6C2be1qbdqdNQsNkvBFsHar6/roKi93fj5AzB7y1py4ZQcttZkq+De9rJkM8vNvV9MBmHGtbXAwQEwPm4C+uE0tgppP4o3kohTbrNjljROnzUn0O/MIMy4rs6UM708Qc2OciKRTHH+mQxCCAFBu4TP2K7Tt6kpTXC7pmRRwskgAQv3qDFz296WIPmKC5Z8kV5IoCk3Sy580Aw8895hzcWHvop5WuJgPvrx+C8XEmAcE1uU9/79o58uAUTFEgJ5L1f3b4v+Trjt+FFlZPwBwaaGGN9mO7gAAAAASUVORK5CYII=",
        [Status.PASS] = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABgAAAAYCAYAAADgdz34AAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAP4SURBVEhLlZZbTFRHGMf3QaP2xYfipbBAhcKevfBARCsFUdhzWVRsUk0s0RhfbNK++GLTNDEmbdr0sqm2JppiY9Fld8+ew1VBxBhNrMYLXmMvoilNQ7WJSNPUAk3X0K/z//acdTkuoF/yZydz+c3MN/8zg2um8JlyvtQe2uQ1tU+khByBuIy6ttoCq9vzh2RoK7xtmikZ6pi/Zw35j68lf7f4hVAWdVKrOib6tEuGXGMNmznKvy6fJQbu9XWEBGQteU2VJF3JKrRhIl9XvShrTYubyudZmOxR2FwzXzLVs4G+deQ1pgbb8uoqFcdWU4leR2V9DWJH2qWiQ1U5Fm5ySObG2SId5wIn1mWFOQX4y9EaWtr2OpUZ9VTYUkNlGJtQ+93NK+dY2CchxeSvsPJsMKe8Qkuiq7g88OcgXR3+nio7NlJxdDWB4YkH91vYVJQmghW+ztAzpQUqiQcpL/IanfrtPNnx0bX99NKRFSSyQGCVxNRlFl6sXle64AwnKJuw+pzmCgrf/MZCEx0eaKf8SDV5dJn7wBxSXD7K8OKWUJ6UUP6dzi22AMfK1594iyb+m2D4yaHvxMor6ZVY7ZN+YCWUpO+InO/y6Goje9pqnE5ITaE42FsjAwz/5a+h9GFj8sy+YJbGlc1IT/hZ0gPQguZltKt/D8Oxgzf63qbFIu9oc/YHU6TsC+GeYBRfp92AlZTE63jLmQPh9zJjDQ2P/8ETHPxRpxe/XZpud4qZ8WDUhT/2BADmt1Tzb3lrQzqvmBSr//zGQYb/PvaAfAmNisSkmdBMpSfANuwUwdsNvdvp5shtOn3vAtsOzsBEASNED8ZHeIL3L4XFhBWTduiUxQy7vLq8xT5kAHf3f8kQxNbTOylX1C06vJzeu/gZ1/366B4fNtKYCXSKmYb8pssbrS2ApWAtrDZXWO7s/csMu/7wB3JHqlg3Hv7EdR9c3cfpcromU5blkz5ddfO34InLx+1dAKZ0b6XkxGMGbjj5Dq3qauTyo+QoVYi7Z7rcQ5yemNLFcESprlbiysVnjg74UmN3jzI08XMPfXztAJdbB3t59U7gJFlXBa4fC58KTyzYZF92BS0rqe7YFt7F6ONxGv4nZc1tZ97lFD4FtRVXUpdd1HHZIXKNV+eKq/ZKoDflqIXiYHEN2PF3coz8iZBw1BSHCzjGGlNc14iiQ1qOZGoXsQp3tJoaT+1gu0budNKO8x/ylZAVLtKCMeL57C8ytOwPjh3Sbmk2nkx/Rz2VHlPJHatim+aJ1Dh9P+nJbNOaAmHlBQszc6QffVMZBQTO4AcfQhmuM9XR5370neHrVN2SGdokdvWpOCPxb4sQyqIObVa3KcLl+h9hzQH3UoYEKwAAAABJRU5ErkJggg==",
        //[Status.WARN] = "⚠️",
        [Status.NONE] = "ℹ️"
    };
    internal static string GetIcon(bool offline, Status status) {
        if (offline) return StatusIcon.GetValueOrDefault(status);

        return status switch {
            Status.PASS => AssetPath.PassIconUrl,
            Status.FAIL => AssetPath.FailIconUrl,
            Status.SKIP => AssetPath.SkipIconUrl,
            //Status.WARN => AssetPaths.WarnIcon,
            Status.NONE => "ℹ️",
            _ => "❓"
        };

    }
    internal static string GetTextIcon(Status status) {
        return status switch {
            Status.PASS => "✓",
            Status.FAIL => "✗",
            Status.SKIP => "⏭",
            Status.NONE => "ℹ️",
            _ => "❓"
        };
    }
}
