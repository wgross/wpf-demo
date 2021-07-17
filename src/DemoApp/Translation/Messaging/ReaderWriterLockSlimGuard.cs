namespace DemoApp.Translation.Messaging
{
    using System;
    using System.Threading;

    /// <summary>
    /// Guards a frameworks ReaderWriterLockSlim instance.
    /// An instance of <see cref="ReaderWriterLockSlim"/>  is created or can be specified
    /// </summary>
    /// <remarks>
    /// An alternaive implementation could provide Extension methods with a generic guard class.
    /// I decided against this becais an instantiable guard can cache the guard instances instead of
    /// always creating new guards.
    /// </remarks>
    public sealed class ReaderWriterLockSlimGuard
    {
        private readonly struct WriteLockGuard : IDisposable
        {
            private readonly ReaderWriterLockSlim guardedRwLock;

            public WriteLockGuard(ReaderWriterLockSlim guardedRwLock) => this.guardedRwLock = guardedRwLock;

            public void Dispose() => this.guardedRwLock.ExitWriteLock();
        }

        private readonly struct ReadLockGuard : IDisposable
        {
            private readonly ReaderWriterLockSlim guardedRwLock;

            public ReadLockGuard(ReaderWriterLockSlim guardedRwLock) => this.guardedRwLock = guardedRwLock;

            public void Dispose() => this.guardedRwLock.ExitReadLock();
        }

        private readonly ReaderWriterLockSlim guardedRwLock;
        private readonly ReadLockGuard readLockGuard;
        private readonly WriteLockGuard writeLockGuard;

        public ReaderWriterLockSlimGuard()
            : this(new(LockRecursionPolicy.NoRecursion))
        { }

        public ReaderWriterLockSlimGuard(ReaderWriterLockSlim guardedRwLock_)
        {
            this.guardedRwLock = guardedRwLock_;
            this.writeLockGuard = new WriteLockGuard(this.guardedRwLock);
            this.readLockGuard = new ReadLockGuard(this.guardedRwLock);
        }

        public IDisposable EnterWriteLock()
        {
            this.guardedRwLock.EnterWriteLock();
            return this.writeLockGuard;
        }

        public IDisposable EnterReadLock()
        {
            this.guardedRwLock.EnterReadLock();
            return this.readLockGuard;
        }
    }
}