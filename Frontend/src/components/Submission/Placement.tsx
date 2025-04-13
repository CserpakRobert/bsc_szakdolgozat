import React from 'react';
import { Chip } from '@mui/material';

interface PlacementProps {
    placement: number;
}

const Placement: React.FC<PlacementProps> = ({ placement }) => {
    if (placement == 1) {
        return <Chip color='secondary' label={`# ${placement}`} />;
    }
    if (placement == 2) {
        return <Chip color='success' label={`# ${placement}`} />;
    }
    if (placement == 3) {
        return <Chip color='warning' label={`# ${placement}`} />;
    }
    return <Chip color='info' label={`# ${placement}`} />;
};

export default Placement;