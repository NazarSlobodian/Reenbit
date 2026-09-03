import { TimeSlotStatus } from "./TimeSlotStatus";

export interface TimeSlot {
  id: string;
  startTime: string;
  endTime: string;
  status: TimeSlotStatus;
}
