export const AvatarIcon = () => {
    return (
        <svg
            width="48"
            height="48"
            viewBox="0 0 48 48"
            fill="none"
            xmlns="http://www.w3.org/2000/svg"
            style={{ borderRadius: '50%' }}
        >
            <circle cx="24" cy="24" r="24" fill="#F3F4F6" />
            <mask id="mask-avatar" style={{ maskType: 'alpha' }} maskUnits="userSpaceOnUse" x="0" y="0" width="48" height="48">
                <circle cx="24" cy="24" r="24" fill="#C4C4C4" />
            </mask>
            <g mask="url(#mask-avatar)">
                <circle cx="24" cy="18" r="7" fill="#4B5563" />
                <path
                    d="M24 28C14.6112 28 7 35.6112 7 45H41C41 35.6112 33.3888 28 24 28Z"
                    fill="#4B5563"
                />
            </g>
        </svg>
    )
}