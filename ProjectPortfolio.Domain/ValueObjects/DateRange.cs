using ProjectPortfolio.Domain.Common;
using ProjectPortfolio.Domain.Exceptions;

namespace ProjectPortfolio.Domain.ValueObjects
{
    /// <summary>
    /// Represents a span of dates between a start and end date, inclusive.
    /// Includes various utilities for manipulation, comparison, and formatting.
    /// </summary>
    public class DateRange : ValueObject
    {
        // Inclusive start date of the range
        public DateTime StartDate { get; private set; }

        // Inclusive end date of the range
        public DateTime EndDate { get; private set; }

        // Parameterless constructor required by EF Core for materialization
        private DateRange() { }

        /// <summary>
        /// Creates a DateRange from two dates.
        /// Validates that start is not after end.
        /// </summary>
        public DateRange(DateTime startDate, DateTime endDate)
        {
            if (startDate > endDate)
                throw new InvalidDateRangeException("Start date cannot be later than end date.");

            StartDate = startDate.Date;
            EndDate = endDate.Date;
        }

        /// <summary>
        /// Creates a DateRange from a start date and duration.
        /// </summary>
        public DateRange(DateTime startDate, TimeSpan duration)
        {
            if (duration.TotalDays < 0)
                throw new InvalidDateRangeException("Duration cannot be negative.");

            StartDate = startDate.Date;
            EndDate = startDate.Date.Add(duration);
        }

        // Static factory for standard two-date constructor
        public static DateRange Create(DateTime startDate, DateTime endDate)
            => new DateRange(startDate, endDate);

        // Create a date range from a start date and number of days
        public static DateRange CreateFromDuration(DateTime startDate, int days)
            => new DateRange(startDate, TimeSpan.FromDays(days));

        // Creates a single-day range
        public static DateRange CreateSingleDay(DateTime date)
            => new DateRange(date, date);

        // Creates a 7-day week range starting from the given date
        public static DateRange CreateWeek(DateTime startOfWeek)
            => new DateRange(startOfWeek, startOfWeek.AddDays(6));

        // Creates a full month range
        public static DateRange CreateMonth(int year, int month)
        {
            var start = new DateTime(year, month, 1);
            var end = start.AddMonths(1).AddDays(-1);
            return new DateRange(start, end);
        }

        // Creates a full year range
        public static DateRange CreateYear(int year)
            => new DateRange(new DateTime(year, 1, 1), new DateTime(year, 12, 31));

        // Gets duration between start and end date
        public TimeSpan Duration => EndDate - StartDate;

        // Gets total number of days in range (inclusive of start and end)
        public int DurationInDays => (int)Duration.TotalDays + 1;

        // Whether the start and end dates are the same
        public bool IsSingleDay => StartDate == EndDate;

        // Returns the midpoint date in the range
        public DateTime MidPoint => StartDate.AddDays(Duration.TotalDays / 2);

        // Status properties for relative date checks
        public bool IsInPast => EndDate < DateTime.Today;
        public bool IsInFuture => StartDate > DateTime.Today;
        public bool IsCurrent => Contains(DateTime.Today);
        public bool HasStarted => StartDate <= DateTime.Today;
        public bool HasEnded => EndDate < DateTime.Today;

        /// <summary>
        /// Checks whether a specific date falls inside the range.
        /// </summary>
        public bool Contains(DateTime date)
        {
            date = date.Date;
            return date >= StartDate && date <= EndDate;
        }

        /// <summary>
        /// Checks whether another DateRange is fully within this one.
        /// </summary>
        public bool Contains(DateRange other)
        {
            return StartDate <= other.StartDate && EndDate >= other.EndDate;
        }

        /// <summary>
        /// Checks if this DateRange overlaps with another.
        /// </summary>
        public bool Overlaps(DateRange other)
        {
            return StartDate <= other.EndDate && EndDate >= other.StartDate;
        }

        /// <summary>
        /// Checks if this DateRange is immediately before or after the other.
        /// </summary>
        public bool IsAdjacent(DateRange other)
        {
            return EndDate.AddDays(1) == other.StartDate || other.EndDate.AddDays(1) == StartDate;
        }

        /// <summary>
        /// Returns a new DateRange that is the overlapping portion between two ranges.
        /// If there is no overlap, returns null.
        /// </summary>
        public DateRange? GetOverlap(DateRange other)
        {
            if (!Overlaps(other))
                return null;

            var overlapStart = StartDate > other.StartDate ? StartDate : other.StartDate;
            var overlapEnd = EndDate < other.EndDate ? EndDate : other.EndDate;

            return new DateRange(overlapStart, overlapEnd);
        }

        /// <summary>
        /// Extends the range either forward or backward by the number of days.
        /// Positive extends the end date; negative extends the start.
        /// </summary>
        public DateRange Extend(int days)
        {
            return days >= 0
                ? new DateRange(StartDate, EndDate.AddDays(days))
                : new DateRange(StartDate.AddDays(days), EndDate);
        }

        /// <summary>
        /// Expands the range to include a specific date if necessary.
        /// </summary>
        public DateRange ExtendToInclude(DateTime date)
        {
            date = date.Date;
            if (Contains(date))
                return this;

            var newStart = date < StartDate ? date : StartDate;
            var newEnd = date > EndDate ? date : EndDate;
            return new DateRange(newStart, newEnd);
        }

        /// <summary>
        /// Expands the range to encompass both this and another range.
        /// </summary>
        public DateRange ExtendToInclude(DateRange other)
        {
            var newStart = StartDate < other.StartDate ? StartDate : other.StartDate;
            var newEnd = EndDate > other.EndDate ? EndDate : other.EndDate;
            return new DateRange(newStart, newEnd);
        }

        /// <summary>
        /// Shifts the range forward or backward in time by a number of days.
        /// </summary>
        public DateRange Shift(int days)
        {
            return new DateRange(StartDate.AddDays(days), EndDate.AddDays(days));
        }

        /// <summary>
        /// Shifts the range to start from a new date, keeping duration the same.
        /// </summary>
        public DateRange ShiftToStart(DateTime newStartDate)
        {
            var duration = Duration;
            return new DateRange(newStartDate, newStartDate.Add(duration));
        }

        /// <summary>
        /// Returns all calendar dates within the range.
        /// </summary>
        public IEnumerable<DateTime> GetDatesInRange()
        {
            for (var date = StartDate; date <= EndDate; date = date.AddDays(1))
                yield return date;
        }

        /// <summary>
        /// Returns all weekdays (Mon–Fri) in the range.
        /// </summary>
        public IEnumerable<DateTime> GetWeekdaysInRange()
        {
            return GetDatesInRange().Where(d => d.DayOfWeek != DayOfWeek.Saturday && d.DayOfWeek != DayOfWeek.Sunday);
        }

        /// <summary>
        /// Returns all weekend days (Sat–Sun) in the range.
        /// </summary>
        public IEnumerable<DateTime> GetWeekendsInRange()
        {
            return GetDatesInRange().Where(d => d.DayOfWeek == DayOfWeek.Saturday || d.DayOfWeek == DayOfWeek.Sunday);
        }

        // Returns total number of weekdays in range
        public int GetWeekdayCount() => GetWeekdaysInRange().Count();

        // Returns total number of weekend days in range
        public int GetWeekendCount() => GetWeekendsInRange().Count();

        /// <summary>
        /// Returns the next date range with same duration starting after current end.
        /// </summary>
        public DateRange GetNextPeriod()
        {
            var duration = Duration;
            return new DateRange(EndDate.AddDays(1), EndDate.AddDays(1).Add(duration));
        }

        /// <summary>
        /// Returns the previous date range with same duration ending before current start.
        /// </summary>
        public DateRange GetPreviousPeriod()
        {
            var duration = Duration;
            return new DateRange(StartDate.Subtract(duration).AddDays(-1), StartDate.AddDays(-1));
        }

        /// <summary>
        /// Used to determine object equality by its value components.
        /// </summary>
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return StartDate;
            yield return EndDate;
        }

        /// <summary>
        /// Formats as a single string.
        /// </summary>
        public override string ToString()
        {
            if (IsSingleDay)
                return StartDate.ToString("yyyy-MM-dd");

            return $"{StartDate:yyyy-MM-dd} to {EndDate:yyyy-MM-dd}";
        }

        /// <summary>
        /// Formats the range using a custom date format.
        /// </summary>
        public string ToString(string format)
        {
            if (IsSingleDay)
                return StartDate.ToString(format);

            return $"{StartDate.ToString(format)} to {EndDate.ToString(format)}";
        }
    }
}
